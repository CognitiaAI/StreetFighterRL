#include <snes/snes.hpp>

namespace SNES {

Interface::Interface()
	: wanttrace(false)
{
}

void Interface::videoRefresh(const uint32_t *data, bool hires, bool interlace, bool overscan) {
}

void Interface::audioSample(int16_t l_sample, int16_t r_sample) {
}

int16_t Interface::inputPoll(bool port, Input::Device device, unsigned index, unsigned id) {
  return 0;
}

void Interface::inputNotify(int index) {
}

void Interface::message(const string &text) {
  print(text, "\n");
}

time_t Interface::currentTime()
{
  return time(0);
}

time_t Interface::randomSeed()
{
  return time(0);
}

int Interface::getBackdropColor()
{
	return -1;
}

void Interface::cpuTrace(uint32_t which, const char *msg) {
}

}
