def get_reward(prev_game_state, new_game_state, action):

    prev_game_state, new_game_state = prev_game_state.reshape((-1,)), new_game_state.reshape((-1,))
    player1_health_prev = prev_game_state[1]
    player2_health_prev = prev_game_state[19]

    player1_health_new = new_game_state[1]
    player2_health_new = new_game_state[19]
    reward = 0
    reward += (player2_health_prev - player2_health_new)
    reward -= (player1_health_prev - player1_health_new)

    if (player2_health_prev - player2_health_new == 0 and new_game_state[2] - new_game_state[20] < 0) and action != 1 and action != 2:
        reward -= 0.1
    if (player2_health_prev - player2_health_new == 0 and new_game_state[2] - new_game_state[20] > 0) and action != 6 and action != 7:
        reward -= 0.1
    return reward
