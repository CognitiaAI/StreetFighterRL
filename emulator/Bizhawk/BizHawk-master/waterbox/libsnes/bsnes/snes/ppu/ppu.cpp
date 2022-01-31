#include <snes/snes.hpp>

#define PPU_CPP
namespace SNES {

PPU ppu;

#include "background/background.cpp"
#include "mmio/mmio.cpp"
#include "screen/screen.cpp"
#include "sprite/sprite.cpp"
#include "window/window.cpp"

void PPU::step(unsigned clocks) {
  clock += clocks;
}

void PPU::synchronize_cpu() {
  if(CPU::Threaded == true) {
    if(clock >= 0 && scheduler.sync != Scheduler::SynchronizeMode::All) co_switch(cpu.thread);
  } else {
    while(clock >= 0) cpu.enter();
  }
}

void PPU::Enter() { ppu.enter(); }

void PPU::enter() {
  while(true) {
    if(scheduler.sync == Scheduler::SynchronizeMode::All) {
      scheduler.exit(Scheduler::ExitReason::SynchronizeEvent);
    }

    scanline();
    add_clocks(68);
    bg1.begin();
    bg2.begin();
    bg3.begin();
    bg4.begin();

    if(vcounter() <= 239) {
      for(signed pixel = -7; pixel <= 255; pixel++) {
        bg1.run(1);
        bg2.run(1);
        bg3.run(1);
        bg4.run(1);
        add_clocks(2);

        bg1.run(0);
        bg2.run(0);
        bg3.run(0);
        bg4.run(0);
        if(pixel >= 0) {
          sprite.run();
          window.run();
          screen.run();
        }
        add_clocks(2);
      }

      add_clocks(14);
      sprite.tilefetch();
    } else {
      add_clocks(1052 + 14 + 136);
    }

    add_clocks(lineclocks() - 68 - 1052 - 14 - 136);
  }
}

void PPU::add_clocks(unsigned clocks) {
  clocks >>= 1;
  while(clocks--) {
    tick(2);
    step(2);
    synchronize_cpu();
  }
}

void PPU::enable() {
  function<uint8 (unsigned)> read = { &PPU::mmio_read, (PPU*)&ppu };
  function<void (unsigned, uint8)> write = { &PPU::mmio_write, (PPU*)&ppu };

  bus.map(Bus::MapMode::Direct, 0x00, 0x3f, 0x2100, 0x213f, read, write);
  bus.map(Bus::MapMode::Direct, 0x80, 0xbf, 0x2100, 0x213f, read, write);
}

void PPU::power() {
  ppu1_version = config.ppu1.version;
  ppu2_version = config.ppu2.version;

	for(int i=0;i<128*1024;i++) vram[i] = 0;
	for(int i=0;i<544;i++) oam[i] = 0;
	for(int i=0;i<512;i++) cgram[i] = 0;

//not sure about this
reset();

}

void PPU::reset() {
  create(Enter, system.cpu_frequency());
  PPUcounter::reset();
  memset(surface, 0, 512 * 512 * sizeof(uint32));

  mmio_reset();
  bg1.reset();
  bg2.reset();
  bg3.reset();
  bg4.reset();
  sprite.reset();
  window.reset();
  screen.reset();

  frame();
}

void PPU::scanline() {
  if(vcounter() == 0) {
    frame();
    bg1.frame();
    bg2.frame();
    bg3.frame();
    bg4.frame();
  }

  bg1.scanline();
  bg2.scanline();
  bg3.scanline();
  bg4.scanline();
  sprite.scanline();
  window.scanline();
  screen.scanline();
}

void PPU::frame() {
  system.frame();
  sprite.frame();

  display.interlace = regs.interlace;
  display.overscan = regs.overscan;
}

void PPU::layer_enable(unsigned layer, unsigned priority, bool enable)
{
	//TODO
}

void PPU::initialize()
{
	vram = (uint8*)interface()->allocSharedMemory("VRAM",128 * 1024);
  oam = (uint8*)interface()->allocSharedMemory("OAM",544);
  cgram = (uint8*)interface()->allocSharedMemory("CGRAM",512);	
  surface = (uint32_t*)alloc_invisible(512 * 512 * sizeof(uint32_t));
  output = surface + 16 * 512;	
}

PPU::PPU() :
bg1(*this, Background::ID::BG1),
bg2(*this, Background::ID::BG2),
bg3(*this, Background::ID::BG3),
bg4(*this, Background::ID::BG4),
sprite(*this),
window(*this),
screen(*this) {

}

PPU::~PPU() {
  abort();
}

}
