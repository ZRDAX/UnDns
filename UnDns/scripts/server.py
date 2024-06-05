# Arquivo: server.py

from flask import Flask, jsonify
from threading import Thread, Lock
import time
from scapy.layers.dns import DNS
from scapy.sendrecv import sniff

app = Flask(__name__)

# Variáveis para rastrear o total de dados e o número de pacotes capturados
total_dados = 0
num_pacotes = 0
lock = Lock()

def dns_sniffer(pkt):
    global total_dados, num_pacotes
    
    with lock:
        if pkt.haslayer(DNS):
            # Calcula o tamanho do pacote DNS
            tamanho_pacote = len(pkt)

            # Atualiza o total de dados e o número de pacotes capturados
            total_dados += tamanho_pacote
            num_pacotes += 1

            if pkt[DNS].qr == 0:  # Consulta DNS
                domain_name = pkt[DNS].qd.qname.decode()  # Obtém o nome do domínio
                print("Consulta DNS: %s -> %s, Tamanho do Pacote: %d bytes" % (pkt['IP'].src, domain_name, tamanho_pacote))
            elif pkt[DNS].qr == 1:  # Resposta DNS
                domain_name = pkt[DNS].qd.qname.decode()  # Obtém o nome do domínio
                print("Resposta DNS: %s -> %s, Tamanho do Pacote: %d bytes" % (pkt['IP'].src, domain_name, tamanho_pacote))

def start_sniffing():
    sniff(filter="udp port 53", prn=dns_sniffer, store=0)

@app.route('/dns_data')
def get_dns_data():
    with lock:
        if num_pacotes > 0:
            media_dados = total_dados / num_pacotes
        else:
            media_dados = 0
        return jsonify(total_dados=total_dados, num_pacotes=num_pacotes, media_dados=media_dados)

if __name__ == '__main__':
    thread_sniffer = Thread(target=start_sniffing)
    thread_sniffer.start()
    
    app.run(debug=True, port=5000)
