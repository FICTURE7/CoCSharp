import socket

def connect():
	s = socket.socket()
	s.connect(('127.0.0.1', 9339))
	return s

def multiconnect(count):
	for i in range(count):
		connect()

def send_data():
	sock = connect()
	sock.sendall(b'lel')

def send_msg(msg):
	sock = connect()
	sock.sendall(msg.encode())

