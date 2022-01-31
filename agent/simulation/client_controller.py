import socket
import json
import numpy as np
from PIL import ImageGrab

from game_entities.game_state import GameState


class ClientController:
    def __init__(self, port):
        self.__server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.__server_socket.bind(("127.0.0.1", port))
        self.__client_socket = None

    def connect(self):
        self.__server_socket.listen(5)
        (self.__client_socket, _) = self.__server_socket.accept()
        print("Connected to game!")

    def send(self, command):
        # This function will send your updated command to Bizhawk so that game reacts according to your command.
        command_dict = command.command_dict
        pay_load = json.dumps(command_dict).encode()
        self.__client_socket.sendall(pay_load)

    def receive(self):
        # receive the game state and return game state
        pay_load = self.__client_socket.recv(4096)
        input_dict = json.loads(pay_load.decode())
        screen_shot = np.array(ImageGrab.grabclipboard())
        game_state = GameState(input_dict)
        return game_state, screen_shot
