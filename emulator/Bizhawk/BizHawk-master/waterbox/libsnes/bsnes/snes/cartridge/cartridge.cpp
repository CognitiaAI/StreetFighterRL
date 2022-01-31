#include <snes/snes.hpp>

#include <nall/crc32.hpp>
#include <nall/sha256.hpp>

#define CARTRIDGE_CPP
namespace SNES {

#include "markup.cpp"

Cartridge cartridge;

void Cartridge::load(Mode cartridge_mode, const char *markup) {
  mode = cartridge_mode;
  region = Region::NTSC;
  ram_size = 0;

  has_bsx_slot   = false;
  has_nss_dip    = false;
  has_superfx    = false;
  has_sa1        = false;
  has_necdsp     = false;
  has_hitachidsp = false;
  has_armdsp     = false;
  has_srtc       = false;
  has_sdd1       = false;
  has_spc7110    = false;
  has_spc7110rtc = false;
  has_obc1       = false;
  has_msu1       = false;
  has_link       = false;

  nvram.reset();

  parse_markup(markup);
//print(markup, "\n\n");

  if(ram_size > 0) {
		uint8* buf = (uint8*)interface()->allocSharedMemory("CARTRIDGE_RAM",ram_size,0xff);
    ram.map(buf, ram_size);
    nvram.append({ "program.ram", ram.data(), ram.size() });
  }

  rom.write_protect(true);
  ram.write_protect(false);

  crc32 = crc32_calculate(rom.data(), rom.size());

  switch((Mode)mode) {
  case Mode::Normal:
  case Mode::BsxSlotted:
    sha256 = nall::sha256(rom.data(), rom.size());
    break;
  case Mode::Bsx:
    sha256 = nall::sha256(bsxflash.memory.data(), bsxflash.memory.size());
    break;
  case Mode::SufamiTurbo:
    sha256 = nall::sha256(sufamiturbo.slotA.rom.data(), sufamiturbo.slotA.rom.size());
    break;
  case Mode::SuperGameBoy:
    #if defined(GAMEBOY)
    sha256 = GameBoy::cartridge.sha256();
    #else
    throw "Game Boy support not present";
    #endif
    break;
  }

  system.load();
  loaded = true;
}

void Cartridge::unload() {
  if(loaded == false) return;

  system.unload();
  rom.reset();
  ram.reset();

  loaded = false;
}

Cartridge::Cartridge()
	: rom("CARTRIDGE_ROM")
	, ram("CARTRIDGE_RAM")
{
  loaded = false;
  unload();
}

Cartridge::~Cartridge() {
  unload();
}

}
