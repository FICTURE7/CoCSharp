import socket
import time

connection = socket.socket()

def connect():
    global connection
    connection.connect(('127.0.0.1', 9339))

def send_fragmentedheader():
    global connection
    for i in range(7):
        connection.send(b'' + chr(97 + i).encode())
        time.sleep(1)

def send_header():
    global connection
    for i in range(7):
        connection.send(b'' + chr(97 + i).encode())

def send_keepalive_packet():
    global connection
    connection.sendall(b'\x27\x7C\x00\x00\x00\x00\x00')

def main():
    print('Connecting to localhost...')
    connect()
    print('Sending incomplete header...')
    send_keepalive_packet()
    send_keepalive_packet()

if __name__ == "__main__":
    main()
