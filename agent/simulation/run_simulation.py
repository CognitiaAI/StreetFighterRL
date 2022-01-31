from client_controller import ClientController
from game_controller import GameController
import sys


def main():
    # if (sys.argv[1]=='1'):
    #     client = ClientController(9999)
    # elif (sys.argv[1]=='2'):
    #     client = ClientController(10000)

    client = ClientController(9999)
    client.connect()
    game_controller = GameController('p1', 'training')
    while(True):
        current_game_state, screen_shot = client.receive()
        bot_command = game_controller.step(current_game_state, screen_shot)
        client.send(bot_command)


if __name__ == '__main__':
    main()