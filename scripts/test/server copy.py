from flask import Flask, jsonify
from threading import Thread, Lock
import logging
from scapy.layers.dns import DNS
from scapy.sendrecv import sniff

# Configurações de logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')

app = Flask(__name__)

# Variáveis para rastrear o total de dados e o número de pacotes capturados
total_dados = 0
num_pacotes = 0
lock = Lock()

DNS_FILTER = "udp port 53"

def dns_sniffer(pkt):
    """Função para capturar e processar pacotes DNS."""
    global total_dados, num_pacotes
    
    if pkt.haslayer(DNS):
        with lock:
            # Calcula o tamanho do pacote DNS
            tamanho_pacote = len(pkt)

            # Atualiza o total de dados e o número de pacotes capturados
            total_dados += tamanho_pacote
            num_pacotes += 1

        # Logging das consultas e respostas DNS
        if pkt[DNS].qr == 0:  # Consulta DNS
            domain_name = pkt[DNS].qd.qname.decode()  # Obtém o nome do domínio
            logging.info(f"Consulta DNS: {pkt['IP'].src} -> {domain_name}, Tamanho do Pacote: {tamanho_pacote} bytes")
        elif pkt[DNS].qr == 1:  # Resposta DNS
            domain_name = pkt[DNS].qd.qname.decode()  # Obtém o nome do domínio
            logging.info(f"Resposta DNS: {pkt['IP'].src} -> {domain_name}, Tamanho do Pacote: {tamanho_pacote} bytes")

def start_sniffing():
    """Inicia a captura de pacotes DNS."""
    logging.info("Iniciando a captura de pacotes DNS...")
    sniff(filter=DNS_FILTER, prn=dns_sniffer, store=0)

@app.route('/dns_data')
def get_dns_data():
    """Rota da API para obter os dados de DNS capturados."""
    with lock:
        if num_pacotes > 0:
            media_dados = total_dados / num_pacotes
        else:
            media_dados = 0
        logging.info(f"Dados retornados: total_dados={total_dados}, num_pacotes={num_pacotes}, media_dados={media_dados}")
        return jsonify(total_dados=total_dados, num_pacotes=num_pacotes, media_dados=media_dados)

if __name__ == '__main__':
    # Inicia a thread para captura de pacotes DNS
    thread_sniffer = Thread(target=start_sniffing)
    thread_sniffer.daemon = True
    thread_sniffer.start()
    
    # Inicia o servidor Flask
    logging.info("Iniciando o servidor Flask...")
    app.run(debug=False, port=5000)
    
### 
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
