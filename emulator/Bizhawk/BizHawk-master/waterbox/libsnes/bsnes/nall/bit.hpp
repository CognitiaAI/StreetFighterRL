#ifndef NALL_BIT_HPP
#define NALL_BIT_HPP

namespace nall {
  template<unsigned bits>
  constexpr inline uintmax_t uclamp(const uintmax_t x) {
    enum : uintmax_t { b = 1ull << (bits - 1), y = b * 2 - 1 };
    return y + ((x - y) & -(x < y));  //min(x, y);
  }

  template<unsigned bits>
  constexpr inline uintmax_t uclip(const uintmax_t x) {
		//zero 17-jun-2015 - revised to use more standard constexpr behaviour
    //enum : uintmax_t { b = 1ull << (bits - 1), m = b * 2 - 1 };
		//return (x & m); //test
    return (x & ((uintmax_t)(((uintmax_t)(1ull << (bits - 1))) * 2 - 1)));
  }

  template<unsigned bits>
  constexpr inline intmax_t sclamp(const intmax_t x) {
		//zero 17-jun-2015 - revised to use more standard constexpr behaviour
    //enum : intmax_t { b = 1ull << (bits - 1), m = b - 1 };
		//(intmax_t)(1ull << (bits - 1)) //b
		//(((intmax_t)(1ull << (bits - 1))) - 1) //m
    //return (x > m) ? m : (x < -b) ? -b : x;
		return (x > (((intmax_t)(1ull << (bits - 1))) - 1)) ? (((intmax_t)(1ull << (bits - 1))) - 1) : (x < -((intmax_t)(1ull << (bits - 1)))) ? -((intmax_t)(1ull << (bits - 1))) : x; //test
  }

  template<unsigned bits>
  constexpr inline intmax_t sclip(const intmax_t x) {
		//zero 17-jun-2015 - revised to use more standard constexpr behaviour
    //enum : uintmax_t { b = 1ull << (bits - 1), m = b * 2 - 1 }; //test
    return ((x & ((uintmax_t)(((uintmax_t)(1ull << (bits - 1))) * 2 - 1))) ^ ((uintmax_t)(1ull << (bits - 1)))) - ((uintmax_t)(1ull << (bits - 1)));
  }

  namespace bit {
    //lowest(0b1110) == 0b0010
    constexpr inline uintmax_t lowest(const uintmax_t x) {
      return x & -x;
    }

    //clear_lowest(0b1110) == 0b1100
    constexpr inline uintmax_t clear_lowest(const uintmax_t x) {
      return x & (x - 1);
    }

    //set_lowest(0b0101) == 0b0111
    constexpr inline uintmax_t set_lowest(const uintmax_t x) {
      return x | (x + 1);
    }

    //count number of bits set in a byte
    inline unsigned count(uintmax_t x) {
      unsigned count = 0;
      do count += x & 1; while(x >>= 1);
      return count;
    }

    //round up to next highest single bit:
    //round(15) == 16, round(16) == 16, round(17) == 32
    inline uintmax_t round(uintmax_t x) {
      if((x & (x - 1)) == 0) return x;
      while(x & (x - 1)) x &= x - 1;
      return x << 1;
    }
  }
}

#endif
