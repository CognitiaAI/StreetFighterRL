from collections import deque
import numpy as np

class BasicDataProvider:
    def __init__(self):
        self.replay_memory_size = 20000
        self.replay_memory = deque([], maxlen=self.replay_memory_size)

    def sample_memories(self, batch_size):
        indices = np.random.permutation(len(self.replay_memory))[:batch_size]
        cols = [[], [], [], [], [], [], []]  # state,image, action, reward, next_state, next_image, continue

        for idx in indices:
            memory = self.replay_memory[idx]
            cols[0].append(memory[0])
            cols[1].append(memory[1])
            cols[2].append(memory[2])
            cols[3].append(memory[3])
            cols[4].append(memory[4])
            cols[5].append(memory[5])
            cols[6].append(memory[6])
            # for col, value in zip(cols, memory):
            # if (c==0):
            # col = value
            # col.append(value)
        cols = [np.array(col) for col in cols]
        return cols[0], cols[1], cols[2], cols[3].reshape((-1, 1)),\
               cols[4], cols[5], cols[6]

    def store(self, memory):
        self.replay_memory.append(memory)

    def store_and_retrieve(self, memory, batch_size):
        self.store(memory)
        if len(self.replay_memory) >= self.replay_memory_size-1:
            return self.sample_memories(batch_size)
        else:
            return None