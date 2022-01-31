struct Interface;

struct System : property<System> {
  enum class Region : unsigned { NTSC = 0, PAL = 1, Autodetect = 2 };
  enum class ExpansionPortDevice : unsigned { None = 0, BSX = 1 };

  void run();
  void runtosave();

  void init();
  void term();
  void load();
  void unload();
  void power();
  void reset();

  void frame();
  void scanline();

  //return *active* system information (settings are cached upon power-on)
  readonly<Region> region;
  readonly<ExpansionPortDevice> expansion;
  readonly<unsigned> cpu_frequency;
  readonly<unsigned> apu_frequency;

  System();

private:
  void runthreadtosave();

  friend class Cartridge;
  friend class Video;
  friend class Audio;
  friend class Input;
};

#include "video.hpp"
#include "audio.hpp"
#include "input.hpp"

#include <snes/config/config.hpp>
#include <snes/scheduler/scheduler.hpp>
#include <snes/random/random.hpp>

extern System system;
