from game_entities.command import Command


class ActionMapper:
    def __init__(self):

        self.__super_move = []

        # TODO: Read the action mapping dictionaries from external json

        self.__actions_mapping = {'jump': ['Up'], 'forward_flip': ['Up', 'Right'], 'forward': ['Right'],
                                  'offensive_crouch': ['Right', 'Down']
            , 'crouch': ['Down'], 'defensive_crouch': ['Down', 'Left'], 'defense': ['Left'], 'back_flip': ['Up', 'Left']
            , 'hard_punch': ['L'], 'hard_kick': ['R'], 'light_punch': ['Y'], 'medium_punch': ['X']
            , 'medium_kick': ['A'], 'jump_hard_kick': ['Up', 'R'], 'jump_medium_kick': ['Up', 'A']
            , 'jump_hard_punch': ['Up', 'L'], 'jump_medium_punch': ['Up', 'X']
            , 'crouch_hard_kick': ['Down', 'R'], 'crouch_medium_kick': ['Down', 'A']
            , 'crouch_hard_punch': ['Down', 'L'], 'crouch_medium_punch': ['Down', 'X']
            , 'crouch_light_punch': ['Down', 'Y']
            , 'Hadookin': ['Down', 'Down', ['Forward', 'Punch']]}
        self.__actions_mapping_num = {0: ['Up'], 1: ['Up', 'Right'], 2: ['Right'], 3: ['Right', 'Down']
            , 4: ['Down'], 5: ['Down', 'Left'], 6: ['Left'], 7: ['Up', 'Left']
            , 8: ['L'], 9: ['R'], 10: ['X']
            , 11: ['A'], 12: ['Up', 'R'], 13: ['Up', 'A']
            , 14: ['Up', 'L'], 15: ['Up', 'X']
            , 16: ['Down', 'R'], 17: ['Down', 'A']
            , 18: ['Down', 'L'], 19: ['Down', 'X']
            , 20: [['Down'], ['Down', 'Right'], ['Right', 'L'], ['Right', 'L']]
            , 21: [['Left'], ['Down', 'Left'], ['Down'], ['Down', 'Right'], ['Right', 'R']]
            , 22: [['Down'], ['Down', 'Left'], ['Left', 'R']]
            , 23: ['Right'] * 20
            , 24: ['Left'] * 20}
        self.__mirror_super_moves = {
             20: [['Down'], ['Down', 'Left'], ['Left', 'L'], ['Left', 'L']],
             21: [['Right'], ['Down', 'Right'], ['Down'], ['Down', 'Left'], ['Left', 'R']],
             22: [['Down'], ['Down', 'Right'], ['Right', 'R']]}

    def get_action_list(self, action, game_state):
        maneuver = self.__actions_mapping_num[action]
        if 19 < action < 23:
            if game_state.player1.x_coord > game_state.player2.x_coord:
                maneuver = self.__mirror_super_moves[action]
        return maneuver
