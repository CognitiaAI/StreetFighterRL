class Interface;

enum class Input : unsigned {
  Up, Down, Left, Right, B, A, Select, Start,
};

struct System : property<System> {
  enum class Revision : unsigned {
    GameBoy,
    SuperGameBoy,
    GameBoyColor,
  };
  readonly<Revision> revision;
  inline bool dmg() const { return (Revision)revision == Revision::GameBoy; }
  inline bool sgb() const { return (Revision)revision == Revision::SuperGameBoy; }
  inline bool cgb() const { return (Revision)revision == Revision::GameBoyColor; }

  struct BootROM {
    static const uint8 dmg[ 256];
    static const uint8 sgb[ 256];
    static const uint8 cgb[2048];
  } bootROM;

  void run();
  void runtosave();
  void runthreadtosave();

  void init();
  void load(Revision);
  void power();

  unsigned clocks_executed;

};

#include <gameboy/interface/interface.hpp>

extern System system;
