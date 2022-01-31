//Hitachi HG51B169

class HitachiDSP : public Coprocessor {
public:

	//zero 01-sep-2014 - dont clobber these when reconstructing!
  static unsigned frequency;
  static uint24 dataROM[1024];

  uint8  dataRAM[3072];
  uint24 stack[8];
  uint16 opcode;
  enum class State : unsigned { Idle, DMA, Execute } state;
  #include "registers.hpp"

  static void Enter();
  void enter();

  void init();
  void load();
  void unload();
  void power();
  void reset();

  //memory.cpp
  uint8 bus_read(unsigned addr);
  void bus_write(unsigned addr, uint8 data);

  uint8 rom_read(unsigned addr);
  void rom_write(unsigned addr, uint8 data);

  uint8 dsp_read(unsigned addr);
  void dsp_write(unsigned addr, uint8 data);

  //opcodes.cpp
  void push();
  void pull();
  unsigned sa();
  unsigned ri();
  unsigned np();
  void exec();

  //registers.cpp
  unsigned reg_read(unsigned n) const;
  void reg_write(unsigned n, unsigned data);
};

extern HitachiDSP hitachidsp;
