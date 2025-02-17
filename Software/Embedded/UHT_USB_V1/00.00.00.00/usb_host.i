#line 1 "usb_host.c"
#line 1 "usb_host.c"

#line 21 "usb_host.c"
 


#line 55 "usb_host.c"
 

#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 

#line 4 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 6 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 9 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
 

#line 16 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
double atof (const auto char *s);

#line 28 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
signed char atob (const auto char *s);


#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
int atoi (const auto char *s);

#line 47 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
long atol (const auto char *s);

#line 58 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
unsigned long atoul (const auto char *s);


#line 71 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
char *btoa (auto signed char value, auto char *s);

#line 83 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
char *itoa (auto int value, auto char *s);

#line 95 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
char *ltoa (auto long value, auto char *s);

#line 107 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
char *ultoa (auto unsigned long value, auto char *s);
 


#line 112 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
 

#line 116 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
#line 118 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"


#line 124 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
int rand (void);

#line 136 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
void srand (auto unsigned int seed);
 
#line 140 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
#line 149 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 151 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
#line 57 "usb_host.c"

#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 3 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"


#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
 

#line 4 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"

typedef unsigned char wchar_t;


#line 10 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
 
typedef signed short int ptrdiff_t;
typedef signed short int ptrdiffram_t;
typedef signed short long int ptrdiffrom_t;


#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
 
typedef unsigned short int size_t;
typedef unsigned short int sizeram_t;
typedef unsigned short long int sizerom_t;


#line 34 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
 
#line 36 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"


#line 41 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
 
#line 43 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"

#line 45 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
#line 5 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 7 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"


#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
#line 22 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"


#line 25 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
#line 27 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

 

#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
void *memcpy (auto void *s1, auto const void *s2, auto size_t n);


#line 55 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
void *memmove (auto void *s1, auto const void *s2, auto size_t n);


#line 67 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strcpy (auto char *s1, auto const char *s2);


#line 83 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strncpy (auto char *s1, auto const char *s2, auto size_t n);


#line 97 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strcat (auto char *s1, auto const char *s2);


#line 113 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strncat (auto char *s1, auto const char *s2, auto size_t n);


#line 128 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char memcmp (auto const void *s1, auto const void *s2, auto size_t n);


#line 141 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strcmp (auto const char *s1, auto const char *s2);


#line 147 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 


#line 161 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strncmp (auto const char *s1, auto const char *s2, auto size_t n);


#line 167 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 


#line 183 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
void *memchr (auto const void *s, auto unsigned char c, auto size_t n);


#line 199 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strchr (auto const char *s, auto unsigned char c);


#line 210 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
size_t strcspn (auto const char *s1, auto const char *s2);


#line 222 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strpbrk (auto const char *s1, auto const char *s2);


#line 238 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strrchr (auto const char *s, auto unsigned char c);


#line 249 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
size_t strspn (auto const char *s1, auto const char *s2);


#line 262 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strstr (auto const char *s1, auto const char *s2);


#line 305 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strtok (auto char *s1, auto const char *s2);


#line 321 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
void *memset (auto void *s, auto unsigned char c, auto size_t n);


#line 339 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
#line 341 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"


#line 349 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
size_t strlen (auto const char *s);


#line 358 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strupr (auto char *s);


#line 367 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strlwr (auto char *s);



 

#line 379 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom void *memcpypgm (auto far  rom void *s1, auto const far  rom void *s2, auto sizerom_t n);


#line 389 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
void *memcpypgm2ram (auto void *s1, auto const far  rom void *s2, auto sizeram_t n);


#line 398 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom void *memcpyram2pgm (auto far  rom void *s1, auto const void *s2, auto sizeram_t n);


#line 407 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom void *memmovepgm (auto far  rom void *s1, auto const far  rom void *s2, auto sizerom_t n);


#line 417 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
void *memmovepgm2ram (auto void *s1, auto const far  rom void *s2, auto sizeram_t n);


#line 426 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom void *memmoveram2pgm (auto far  rom void *s1, auto const void *s2, auto sizeram_t n);


#line 434 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strcpypgm (auto far  rom char *s1, auto const far  rom char *s2);


#line 443 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strcpypgm2ram (auto char *s1, auto const far  rom char *s2);


#line 451 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strcpyram2pgm (auto far  rom char *s1, auto const char *s2);


#line 460 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strncpypgm (auto far  rom char *s1, auto const far  rom char *s2, auto sizerom_t n);


#line 470 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strncpypgm2ram (auto char *s1, auto const far  rom char *s2, auto sizeram_t n);


#line 479 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strncpyram2pgm (auto far  rom char *s1, auto const char *s2, auto sizeram_t n);


#line 487 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strcatpgm (auto far  rom char *s1, auto const far  rom char *s2);


#line 496 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strcatpgm2ram (auto char *s1, auto const far  rom char *s2);


#line 504 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strcatram2pgm (auto far  rom char *s1, auto const char *s2);


#line 513 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strncatpgm (auto far  rom char *s1, auto const far  rom char *s2, auto sizerom_t n);


#line 523 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strncatpgm2ram (auto char *s1, auto const far  rom char *s2, auto sizeram_t n);


#line 532 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strncatram2pgm (auto far  rom char *s1, auto const char *s2, auto sizeram_t n);


#line 541 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char memcmppgm (auto far  rom void *s1, auto const far  rom void *s2, auto sizerom_t n);


#line 551 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char memcmppgm2ram (auto void *s1, auto const far  rom void *s2, auto sizeram_t n);


#line 560 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char memcmpram2pgm (auto far  rom void *s1, auto const void *s2, auto sizeram_t n);


#line 568 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strcmppgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 577 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strcmppgm2ram (auto const char *s1, auto const far  rom char *s2);


#line 585 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strcmpram2pgm (auto const far  rom char *s1, auto const char *s2);


#line 594 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strncmppgm (auto const far  rom char *s1, auto const far  rom char *s2, auto sizerom_t n);


#line 604 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strncmppgm2ram (auto char *s1, auto const far  rom char *s2, auto sizeram_t n);


#line 613 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
signed char strncmpram2pgm (auto far  rom char *s1, auto const char *s2, auto sizeram_t n);


#line 622 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *memchrpgm (auto const far  rom char *s, auto const unsigned char c, auto sizerom_t n);


#line 631 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strchrpgm (auto const far  rom char *s, auto unsigned char c);


#line 639 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
sizerom_t strcspnpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 647 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
sizerom_t strcspnpgmram (auto const far  rom char *s1, auto const char *s2);


#line 655 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
sizeram_t strcspnrampgm (auto const char *s1, auto const far  rom char *s2);


#line 663 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strpbrkpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 671 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strpbrkpgmram (auto const far  rom char *s1, auto const char *s2);


#line 679 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strpbrkrampgm (auto const char *s1, auto const far  rom char *s2);


#line 688 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
 


#line 696 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
sizerom_t strspnpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 704 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
sizerom_t strspnpgmram (auto const far  rom char *s1, auto const char *s2);


#line 712 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
sizeram_t strspnrampgm (auto const char *s1, auto const far  rom char *s2);


#line 720 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strstrpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 729 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strstrpgmram (auto const far  rom char *s1, auto const char *s2);


#line 737 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strstrrampgm (auto const char *s1, auto const far  rom char *s2);


#line 745 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strtokpgm (auto far  rom char *s1, auto const far  rom char *s2);


#line 754 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
char *strtokpgmram (auto char *s1, auto const far  rom char *s2);


#line 762 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strtokrampgm (auto far  rom char *s1, auto const char *s2);


#line 771 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom void *memsetpgm (auto far  rom void *s, auto unsigned char c, auto sizerom_t n);


#line 778 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *struprpgm (auto far  rom char *s);


#line 785 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
far  rom char *strlwrpgm (auto far  rom char *s);


#line 792 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
 
sizerom_t strlenpgm (auto const far  rom char *s);

#line 796 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 798 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 805 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 814 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 816 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 58 "usb_host.c"

#line 1 "./GenericTypeDefs.h"

#line 43 "./GenericTypeDefs.h"
 


#line 47 "./GenericTypeDefs.h"

typedef enum _BOOL { FALSE = 0, TRUE } BOOL;	

#line 53 "./GenericTypeDefs.h"

#line 55 "./GenericTypeDefs.h"
#line 56 "./GenericTypeDefs.h"
#line 57 "./GenericTypeDefs.h"

typedef unsigned char		BYTE;				
typedef unsigned short int	WORD;				
typedef unsigned long		DWORD;				
typedef unsigned long long	QWORD;				
typedef signed char			CHAR;				
typedef signed short int	SHORT;				
typedef signed long			LONG;				
typedef signed long long	LONGLONG;			

 
typedef void                VOID;

typedef char                CHAR8;
typedef unsigned char       UCHAR8;

 



typedef signed int          INT;
typedef signed char         INT8;
typedef signed short int    INT16;
typedef signed long int     INT32;
typedef signed long long    INT64;

typedef unsigned int        UINT;
typedef unsigned char       UINT8;
typedef unsigned short int  UINT16;
typedef unsigned long int   UINT32;  
typedef unsigned long long  UINT64;

typedef union _BYTE_VAL
{
    BYTE Val;
    struct
    {
        unsigned char b0:1;
        unsigned char b1:1;
        unsigned char b2:1;
        unsigned char b3:1;
        unsigned char b4:1;
        unsigned char b5:1;
        unsigned char b6:1;
        unsigned char b7:1;
    } bits;
} BYTE_VAL, BYTE_BITS;

typedef union _WORD_VAL
{
    WORD Val;
    BYTE v[2];
    struct
    {
        BYTE LB;
        BYTE HB;
    } byte;
    struct
    {
        unsigned char b0:1;
        unsigned char b1:1;
        unsigned char b2:1;
        unsigned char b3:1;
        unsigned char b4:1;
        unsigned char b5:1;
        unsigned char b6:1;
        unsigned char b7:1;
        unsigned char b8:1;
        unsigned char b9:1;
        unsigned char b10:1;
        unsigned char b11:1;
        unsigned char b12:1;
        unsigned char b13:1;
        unsigned char b14:1;
        unsigned char b15:1;
    } bits;
} WORD_VAL, WORD_BITS;

typedef union _DWORD_VAL
{
    DWORD Val;
	WORD w[2];
    BYTE v[4];
    struct
    {
        WORD LW;
        WORD HW;
    } word;
    struct
    {
        BYTE LB;
        BYTE HB;
        BYTE UB;
        BYTE MB;
    } byte;
    struct
    {
        WORD_VAL low;
        WORD_VAL high;
    }wordUnion;
    struct
    {
        unsigned char b0:1;
        unsigned char b1:1;
        unsigned char b2:1;
        unsigned char b3:1;
        unsigned char b4:1;
        unsigned char b5:1;
        unsigned char b6:1;
        unsigned char b7:1;
        unsigned char b8:1;
        unsigned char b9:1;
        unsigned char b10:1;
        unsigned char b11:1;
        unsigned char b12:1;
        unsigned char b13:1;
        unsigned char b14:1;
        unsigned char b15:1;
        unsigned char b16:1;
        unsigned char b17:1;
        unsigned char b18:1;
        unsigned char b19:1;
        unsigned char b20:1;
        unsigned char b21:1;
        unsigned char b22:1;
        unsigned char b23:1;
        unsigned char b24:1;
        unsigned char b25:1;
        unsigned char b26:1;
        unsigned char b27:1;
        unsigned char b28:1;
        unsigned char b29:1;
        unsigned char b30:1;
        unsigned char b31:1;
    } bits;
} DWORD_VAL;

typedef union _QWORD_VAL
{
    QWORD Val;
	DWORD d[2];
	WORD w[4];
    BYTE v[8];
    struct
    {
        DWORD LD;
        DWORD HD;
    } dword;
    struct
    {
        WORD LW;
        WORD HW;
        WORD UW;
        WORD MW;
    } word;
    struct
    {
        unsigned char b0:1;
        unsigned char b1:1;
        unsigned char b2:1;
        unsigned char b3:1;
        unsigned char b4:1;
        unsigned char b5:1;
        unsigned char b6:1;
        unsigned char b7:1;
        unsigned char b8:1;
        unsigned char b9:1;
        unsigned char b10:1;
        unsigned char b11:1;
        unsigned char b12:1;
        unsigned char b13:1;
        unsigned char b14:1;
        unsigned char b15:1;
        unsigned char b16:1;
        unsigned char b17:1;
        unsigned char b18:1;
        unsigned char b19:1;
        unsigned char b20:1;
        unsigned char b21:1;
        unsigned char b22:1;
        unsigned char b23:1;
        unsigned char b24:1;
        unsigned char b25:1;
        unsigned char b26:1;
        unsigned char b27:1;
        unsigned char b28:1;
        unsigned char b29:1;
        unsigned char b30:1;
        unsigned char b31:1;
        unsigned char b32:1;
        unsigned char b33:1;
        unsigned char b34:1;
        unsigned char b35:1;
        unsigned char b36:1;
        unsigned char b37:1;
        unsigned char b38:1;
        unsigned char b39:1;
        unsigned char b40:1;
        unsigned char b41:1;
        unsigned char b42:1;
        unsigned char b43:1;
        unsigned char b44:1;
        unsigned char b45:1;
        unsigned char b46:1;
        unsigned char b47:1;
        unsigned char b48:1;
        unsigned char b49:1;
        unsigned char b50:1;
        unsigned char b51:1;
        unsigned char b52:1;
        unsigned char b53:1;
        unsigned char b54:1;
        unsigned char b55:1;
        unsigned char b56:1;
        unsigned char b57:1;
        unsigned char b58:1;
        unsigned char b59:1;
        unsigned char b60:1;
        unsigned char b61:1;
        unsigned char b62:1;
        unsigned char b63:1;
    } bits;
} QWORD_VAL;

#line 282 "./GenericTypeDefs.h"
#line 59 "usb_host.c"

#line 1 "./usb.h"


#line 7 "./usb.h"
 


#line 63 "./usb.h"
 

#line 97 "./usb.h"
 




#line 103 "./usb.h"









#line 1 "./usb_config.h"

#line 43 "./usb_config.h"
 


#line 47 "./usb_config.h"
 


#line 51 "./usb_config.h"

 
#line 54 "./usb_config.h"
								
								
								
								
								
									
#line 61 "./usb_config.h"
#line 62 "./usb_config.h"




#line 67 "./usb_config.h"
#line 68 "./usb_config.h"



#line 72 "./usb_config.h"
#line 73 "./usb_config.h"




#line 78 "./usb_config.h"





#line 84 "./usb_config.h"

 
#line 87 "./usb_config.h"


#line 90 "./usb_config.h"





#line 96 "./usb_config.h"


#line 99 "./usb_config.h"

#line 101 "./usb_config.h"


#line 104 "./usb_config.h"










 
#line 116 "./usb_config.h"

 

 
#line 121 "./usb_config.h"
#line 122 "./usb_config.h"
#line 123 "./usb_config.h"
#line 124 "./usb_config.h"
#line 125 "./usb_config.h"
#line 126 "./usb_config.h"

 

#line 130 "./usb_config.h"
#line 112 "./usb.h"
             

#line 1 "./usb_common.h"

#line 43 "./usb_common.h"
 


#line 79 "./usb_common.h"
 





#line 86 "./usb_common.h"


#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

#line 9 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
 


#line 13 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

#line 15 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 16 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

#line 18 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 19 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

 
#line 23 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 26 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 27 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 28 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 29 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

#line 31 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 32 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 33 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

#line 35 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 36 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 37 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 40 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 41 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

 
#line 44 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 45 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 46 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

 
#line 49 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"
#line 50 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"

#line 52 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/limits.h"











#line 88 "./usb_common.h"

#line 1 "./GenericTypeDefs.h"

#line 43 "./GenericTypeDefs.h"
 

#line 53 "./GenericTypeDefs.h"
#line 282 "./GenericTypeDefs.h"
#line 89 "./usb_common.h"

#line 1 "./usb_config.h"

#line 43 "./usb_config.h"
 


#line 47 "./usb_config.h"
 

#line 130 "./usb_config.h"
#line 90 "./usb_common.h"











#line 102 "./usb_common.h"
#line 103 "./usb_common.h"
#line 104 "./usb_common.h"
#line 105 "./usb_common.h"
#line 106 "./usb_common.h"
#line 107 "./usb_common.h"
#line 108 "./usb_common.h"
#line 109 "./usb_common.h"
#line 110 "./usb_common.h"
#line 111 "./usb_common.h"
#line 112 "./usb_common.h"
#line 113 "./usb_common.h"
#line 114 "./usb_common.h"
#line 115 "./usb_common.h"
#line 116 "./usb_common.h"

#line 118 "./usb_common.h"
#line 119 "./usb_common.h"
#line 120 "./usb_common.h"
#line 121 "./usb_common.h"
#line 122 "./usb_common.h"
#line 123 "./usb_common.h"
#line 124 "./usb_common.h"
#line 125 "./usb_common.h"
#line 126 "./usb_common.h"
#line 127 "./usb_common.h"
#line 128 "./usb_common.h"
#line 129 "./usb_common.h"



#line 133 "./usb_common.h"
#line 134 "./usb_common.h"
#line 135 "./usb_common.h"
#line 136 "./usb_common.h"
#line 137 "./usb_common.h"
#line 138 "./usb_common.h"
#line 139 "./usb_common.h"
#line 140 "./usb_common.h"
#line 141 "./usb_common.h"
#line 142 "./usb_common.h"
#line 143 "./usb_common.h"
#line 144 "./usb_common.h"

#line 146 "./usb_common.h"

#line 148 "./usb_common.h"










#line 173 "./usb_common.h"
 

typedef union
{
    BYTE    bitmap;
    struct
    {
        BYTE ep_num:    4;
        BYTE zero_pkt:  1;
        BYTE dts:       1;
        BYTE force_dts: 1;
        BYTE direction: 1;
    }field;

} TRANSFER_FLAGS;




#line 195 "./usb_common.h"
 
#line 197 "./usb_common.h"
#line 198 "./usb_common.h"
#line 199 "./usb_common.h"
#line 200 "./usb_common.h"
#line 201 "./usb_common.h"
#line 202 "./usb_common.h"
#line 203 "./usb_common.h"
#line 204 "./usb_common.h"
#line 205 "./usb_common.h"
#line 206 "./usb_common.h"
#line 207 "./usb_common.h"
#line 208 "./usb_common.h"
#line 209 "./usb_common.h"
#line 210 "./usb_common.h"
#line 211 "./usb_common.h"
#line 212 "./usb_common.h"



#line 218 "./usb_common.h"
 
#line 220 "./usb_common.h"
#line 221 "./usb_common.h"
#line 222 "./usb_common.h"
#line 223 "./usb_common.h"
#line 224 "./usb_common.h"
#line 225 "./usb_common.h"
#line 226 "./usb_common.h"
#line 227 "./usb_common.h"
#line 228 "./usb_common.h"
#line 229 "./usb_common.h"
#line 230 "./usb_common.h"



#line 237 "./usb_common.h"
 
#line 239 "./usb_common.h"




#line 248 "./usb_common.h"
 

typedef enum
{
    
    EVENT_NONE = 0,
    
    
    
    
    EVENT_TRANSFER,
    
    
    
    EVENT_SOF,                  
    
    
    EVENT_RESUME,
    
    
    
    EVENT_SUSPEND,
                  
    
    
    EVENT_RESET,                
    
    
    
    
    EVENT_ATTACH,      
             
    
    
    EVENT_DETACH,               
    
    
    EVENT_HUB_ATTACH,           
    
    
    EVENT_STALL,     
               
    
    
    EVENT_SETUP,                
    
    
    EVENT_VBUS_SES_REQUEST,     
    
    
    
    
    EVENT_VBUS_OVERCURRENT,     
    
    
    
    
    
    EVENT_VBUS_REQUEST_POWER,   
    
    
    
    
    EVENT_VBUS_RELEASE_POWER,   
    
    
    
    
    
    
    
    
    EVENT_VBUS_POWER_AVAILABLE, 
    
    
    
    EVENT_UNSUPPORTED_DEVICE,   
    
    
    
    EVENT_CANNOT_ENUMERATE,     
    
    
    
    EVENT_CLIENT_INIT_ERROR,    
    
    
    
    
    
    EVENT_OUT_OF_MEMORY,        
    
    
    EVENT_UNSPECIFIED_ERROR,    

    
    EVENT_CONFIGURED,

    
    EVENT_SET_DESCRIPTOR,

    
    
    
    
    EVENT_EP0_REQUEST,

    
    EVENT_GENERIC_BASE  = 100,      

    EVENT_MSD_BASE      = 200,      

    EVENT_HID_BASE      = 300,      

    EVENT_PRINTER_BASE  = 400,      
    
    EVENT_CDC_BASE      = 500,      

    EVENT_CHARGER_BASE  = 600,      
    
	EVENT_USER_BASE     = 10000,    
                                    

    
    
    EVENT_BUS_ERROR     = 65535U    

} USB_EVENT;




#line 385 "./usb_common.h"
 

typedef struct _transfer_event_data
{
    TRANSFER_FLAGS  flags;          
    UINT32          size;           
    BYTE            pid;            

} USB_TRANSFER_EVENT_DATA;




#line 402 "./usb_common.h"
 

typedef struct _vbus_power_data
{
    BYTE            port;           
    BYTE            current;        
} USB_VBUS_POWER_EVENT_DATA;




#line 417 "./usb_common.h"
 









#line 461 "./usb_common.h"
 

typedef BOOL (*USB_EVENT_HANDLER) ( USB_EVENT event, void *data, unsigned int size );









#line 496 "./usb_common.h"
 

   
#line 500 "./usb_common.h"
#line 501 "./usb_common.h"
#line 502 "./usb_common.h"
#line 504 "./usb_common.h"

#line 506 "./usb_common.h"
#line 508 "./usb_common.h"
#line 509 "./usb_common.h"
#line 510 "./usb_common.h"
#line 511 "./usb_common.h"
#line 512 "./usb_common.h"
#line 513 "./usb_common.h"
#line 515 "./usb_common.h"
#line 517 "./usb_common.h"
#line 518 "./usb_common.h"
#line 519 "./usb_common.h"



#line 545 "./usb_common.h"
 

    
#line 549 "./usb_common.h"
#line 550 "./usb_common.h"
#line 551 "./usb_common.h"
#line 553 "./usb_common.h"
#line 555 "./usb_common.h"
#line 556 "./usb_common.h"
#line 557 "./usb_common.h"
#line 558 "./usb_common.h"
#line 559 "./usb_common.h"
#line 560 "./usb_common.h"
#line 562 "./usb_common.h"
#line 564 "./usb_common.h"
#line 565 "./usb_common.h"
#line 566 "./usb_common.h"


#line 569 "./usb_common.h"

#line 571 "./usb_common.h"
 

#line 114 "./usb.h"
         
#line 1 "./usb_ch9.h"

#line 45 "./usb_ch9.h"
 


#line 78 "./usb_ch9.h"
 




#line 84 "./usb_ch9.h"









#line 94 "./usb_ch9.h"
#line 95 "./usb_ch9.h"
#line 96 "./usb_ch9.h"
#line 97 "./usb_ch9.h"
#line 98 "./usb_ch9.h"
#line 99 "./usb_ch9.h"
#line 100 "./usb_ch9.h"
#line 101 "./usb_ch9.h"
#line 102 "./usb_ch9.h"



#line 109 "./usb_ch9.h"
 
typedef struct __attribute__ ((packed)) _USB_DEVICE_DESCRIPTOR
{
    BYTE bLength;               
    BYTE bDescriptorType;       
    WORD bcdUSB;                
    BYTE bDeviceClass;          
    BYTE bDeviceSubClass;       
    BYTE bDeviceProtocol;       
    BYTE bMaxPacketSize0;       
    WORD idVendor;              
    WORD idProduct;             
    WORD bcdDevice;             
    BYTE iManufacturer;         
    BYTE iProduct;              
    BYTE iSerialNumber;         
    BYTE bNumConfigurations;    
} USB_DEVICE_DESCRIPTOR;




#line 135 "./usb_ch9.h"
 
typedef struct __attribute__ ((packed)) _USB_CONFIGURATION_DESCRIPTOR
{
    BYTE bLength;               
    BYTE bDescriptorType;       
    WORD wTotalLength;          
    BYTE bNumInterfaces;        
    BYTE bConfigurationValue;   
    BYTE iConfiguration;        
    BYTE bmAttributes;          
    BYTE bMaxPower;             
} USB_CONFIGURATION_DESCRIPTOR;


#line 150 "./usb_ch9.h"
#line 151 "./usb_ch9.h"
#line 152 "./usb_ch9.h"




#line 160 "./usb_ch9.h"
 
typedef struct __attribute__ ((packed)) _USB_INTERFACE_DESCRIPTOR
{
    BYTE bLength;               
    BYTE bDescriptorType;       
    BYTE bInterfaceNumber;      
    BYTE bAlternateSetting;     
    BYTE bNumEndpoints;         
    BYTE bInterfaceClass;       
    BYTE bInterfaceSubClass;    
    BYTE bInterfaceProtocol;    
    BYTE iInterface;            
} USB_INTERFACE_DESCRIPTOR;




#line 181 "./usb_ch9.h"
 
typedef struct __attribute__ ((packed)) _USB_ENDPOINT_DESCRIPTOR
{
    BYTE bLength;               
    BYTE bDescriptorType;       
    BYTE bEndpointAddress;      
    BYTE bmAttributes;          
    WORD wMaxPacketSize;        
    BYTE bInterval;             
} USB_ENDPOINT_DESCRIPTOR;



#line 195 "./usb_ch9.h"
#line 196 "./usb_ch9.h"







#line 204 "./usb_ch9.h"
#line 205 "./usb_ch9.h"
#line 206 "./usb_ch9.h"
#line 207 "./usb_ch9.h"


#line 210 "./usb_ch9.h"
#line 211 "./usb_ch9.h"
#line 212 "./usb_ch9.h"
#line 213 "./usb_ch9.h"


#line 216 "./usb_ch9.h"
#line 217 "./usb_ch9.h"
#line 218 "./usb_ch9.h"


#line 221 "./usb_ch9.h"
#line 222 "./usb_ch9.h"
#line 223 "./usb_ch9.h"
#line 224 "./usb_ch9.h"
#line 225 "./usb_ch9.h"
#line 226 "./usb_ch9.h"
#line 227 "./usb_ch9.h"




#line 235 "./usb_ch9.h"
 
typedef struct __attribute__ ((packed)) _USB_OTG_DESCRIPTOR
{
    BYTE bLength;               
    BYTE bDescriptorType;       
    BYTE bmAttributes;          
} USB_OTG_DESCRIPTOR;


















typedef struct __attribute__ ((packed)) _USB_STRING_DSC
{
    BYTE   bLength;             
    BYTE   bDescriptorType;     

} USB_STRING_DESCRIPTOR;













typedef struct __attribute__ ((packed)) _USB_DEVICE_QUALIFIER_DESCRIPTOR
{
    BYTE bLength;               
    BYTE bType;                 
    WORD bcdUSB;                
    BYTE bDeviceClass;          
    BYTE bDeviceSubClass;       
    BYTE bDeviceProtocol;       
    BYTE bMaxPacketSize0;       
    BYTE bNumConfigurations;    
    BYTE bReserved;             

} USB_DEVICE_QUALIFIER_DESCRIPTOR;












typedef struct __attribute__ ((packed)) SetupPkt
{
    union                           
    {                               
        BYTE bmRequestType;         
        struct
        {
            BYTE    recipient:  5;  
            BYTE    type:       2;  
            BYTE    direction:  1;  
        };
    }requestInfo;

    BYTE    bRequest;               
    UINT16  wValue;                 
    UINT16  wIndex;                 
    UINT16  wLength;                

} SETUP_PKT, *PSETUP_PKT;










#line 334 "./usb_ch9.h"
#line 335 "./usb_ch9.h"
#line 336 "./usb_ch9.h"
#line 337 "./usb_ch9.h"
#line 338 "./usb_ch9.h"
#line 339 "./usb_ch9.h"
#line 340 "./usb_ch9.h"
#line 341 "./usb_ch9.h"
#line 342 "./usb_ch9.h"
#line 343 "./usb_ch9.h"
#line 344 "./usb_ch9.h"
#line 345 "./usb_ch9.h"
#line 346 "./usb_ch9.h"
#line 347 "./usb_ch9.h"
#line 348 "./usb_ch9.h"
#line 349 "./usb_ch9.h"

#line 351 "./usb_ch9.h"
#line 352 "./usb_ch9.h"




#line 357 "./usb_ch9.h"
#line 358 "./usb_ch9.h"
#line 359 "./usb_ch9.h"




#line 364 "./usb_ch9.h"
#line 365 "./usb_ch9.h"



#line 369 "./usb_ch9.h"
#line 370 "./usb_ch9.h"



#line 374 "./usb_ch9.h"
#line 375 "./usb_ch9.h"
#line 376 "./usb_ch9.h"
#line 377 "./usb_ch9.h"
#line 378 "./usb_ch9.h"
#line 379 "./usb_ch9.h"
#line 380 "./usb_ch9.h"
#line 381 "./usb_ch9.h"
#line 382 "./usb_ch9.h"
#line 383 "./usb_ch9.h"
#line 384 "./usb_ch9.h"

#line 386 "./usb_ch9.h"
#line 387 "./usb_ch9.h"
#line 388 "./usb_ch9.h"



#line 392 "./usb_ch9.h"
#line 393 "./usb_ch9.h"
#line 394 "./usb_ch9.h"
#line 395 "./usb_ch9.h"
#line 396 "./usb_ch9.h"
#line 397 "./usb_ch9.h"
#line 398 "./usb_ch9.h"
#line 399 "./usb_ch9.h"
#line 400 "./usb_ch9.h"



#line 404 "./usb_ch9.h"
#line 405 "./usb_ch9.h"
#line 406 "./usb_ch9.h"



#line 410 "./usb_ch9.h"
#line 411 "./usb_ch9.h"
#line 412 "./usb_ch9.h"
#line 413 "./usb_ch9.h"


#line 416 "./usb_ch9.h"
#line 417 "./usb_ch9.h"
#line 418 "./usb_ch9.h"



#line 422 "./usb_ch9.h"


#line 425 "./usb_ch9.h"

#line 427 "./usb_ch9.h"
 

#line 115 "./usb.h"
            
#line 1 "./usb_hal.h"

#line 41 "./usb_hal.h"
 


#line 84 "./usb_hal.h"
 


#line 88 "./usb_hal.h"


#line 1 "./usb_common.h"

#line 43 "./usb_common.h"
 


#line 79 "./usb_common.h"
 





#line 173 "./usb_common.h"

#line 195 "./usb_common.h"

#line 218 "./usb_common.h"

#line 237 "./usb_common.h"

#line 248 "./usb_common.h"

#line 385 "./usb_common.h"

#line 402 "./usb_common.h"

#line 417 "./usb_common.h"

#line 461 "./usb_common.h"

#line 496 "./usb_common.h"
#line 500 "./usb_common.h"
#line 501 "./usb_common.h"
#line 502 "./usb_common.h"
#line 504 "./usb_common.h"

#line 506 "./usb_common.h"
#line 508 "./usb_common.h"
#line 509 "./usb_common.h"
#line 511 "./usb_common.h"
#line 512 "./usb_common.h"
#line 513 "./usb_common.h"
#line 515 "./usb_common.h"
#line 517 "./usb_common.h"
#line 518 "./usb_common.h"
#line 519 "./usb_common.h"

#line 545 "./usb_common.h"
#line 549 "./usb_common.h"
#line 550 "./usb_common.h"
#line 551 "./usb_common.h"
#line 553 "./usb_common.h"
#line 555 "./usb_common.h"
#line 556 "./usb_common.h"
#line 558 "./usb_common.h"
#line 559 "./usb_common.h"
#line 560 "./usb_common.h"
#line 562 "./usb_common.h"
#line 564 "./usb_common.h"
#line 565 "./usb_common.h"
#line 566 "./usb_common.h"
#line 569 "./usb_common.h"

#line 571 "./usb_common.h"
 

#line 90 "./usb_hal.h"


#line 93 "./usb_hal.h"

#line 96 "./usb_hal.h"

#line 127 "./usb_hal.h"

#line 130 "./usb_hal.h"

#line 164 "./usb_hal.h"

#line 168 "./usb_hal.h"
#line 170 "./usb_hal.h"
#line 172 "./usb_hal.h"
#line 175 "./usb_hal.h"

#line 183 "./usb_hal.h"

#line 191 "./usb_hal.h"

#line 215 "./usb_hal.h"

#line 241 "./usb_hal.h"

#line 253 "./usb_hal.h"

#line 282 "./usb_hal.h"

#line 288 "./usb_hal.h"

#line 328 "./usb_hal.h"

#line 363 "./usb_hal.h"

#line 367 "./usb_hal.h"

#line 400 "./usb_hal.h"

#line 404 "./usb_hal.h"

#line 433 "./usb_hal.h"

#line 438 "./usb_hal.h"

#line 477 "./usb_hal.h"

#line 537 "./usb_hal.h"

#line 577 "./usb_hal.h"
#line 583 "./usb_hal.h"
#line 590 "./usb_hal.h"

#line 597 "./usb_hal.h"
#line 601 "./usb_hal.h"

#line 627 "./usb_hal.h"
#line 632 "./usb_hal.h"
#line 1 "./usb_hal_pic18.h"

#line 55 "./usb_hal_pic18.h"
 


#line 96 "./usb_hal_pic18.h"
 


#line 116 "./usb_hal_pic18.h"
 
#line 118 "./usb_hal_pic18.h"


#line 135 "./usb_hal_pic18.h"
 

#line 140 "./usb_hal_pic18.h"
#line 141 "./usb_hal_pic18.h"


#line 158 "./usb_hal_pic18.h"
 
#line 160 "./usb_hal_pic18.h"

#line 162 "./usb_hal_pic18.h"
#line 164 "./usb_hal_pic18.h"
#line 165 "./usb_hal_pic18.h"
#line 166 "./usb_hal_pic18.h"
#line 167 "./usb_hal_pic18.h"
#line 169 "./usb_hal_pic18.h"
#line 170 "./usb_hal_pic18.h"
#line 171 "./usb_hal_pic18.h"


#line 174 "./usb_hal_pic18.h"
#line 175 "./usb_hal_pic18.h"
#line 176 "./usb_hal_pic18.h"
#line 177 "./usb_hal_pic18.h"
#line 178 "./usb_hal_pic18.h"
#line 181 "./usb_hal_pic18.h"

#line 183 "./usb_hal_pic18.h"


#line 186 "./usb_hal_pic18.h"

#line 188 "./usb_hal_pic18.h"
#line 189 "./usb_hal_pic18.h"
#line 190 "./usb_hal_pic18.h"
#line 191 "./usb_hal_pic18.h"

#line 193 "./usb_hal_pic18.h"
#line 194 "./usb_hal_pic18.h"
#line 195 "./usb_hal_pic18.h"
#line 196 "./usb_hal_pic18.h"

#line 198 "./usb_hal_pic18.h"
#line 199 "./usb_hal_pic18.h"
#line 200 "./usb_hal_pic18.h"
#line 201 "./usb_hal_pic18.h"

#line 203 "./usb_hal_pic18.h"
#line 204 "./usb_hal_pic18.h"
#line 205 "./usb_hal_pic18.h"
#line 206 "./usb_hal_pic18.h"

#line 208 "./usb_hal_pic18.h"
#line 209 "./usb_hal_pic18.h"
#line 210 "./usb_hal_pic18.h"
#line 211 "./usb_hal_pic18.h"

#line 213 "./usb_hal_pic18.h"
#line 214 "./usb_hal_pic18.h"
#line 215 "./usb_hal_pic18.h"
#line 216 "./usb_hal_pic18.h"

#line 218 "./usb_hal_pic18.h"
#line 219 "./usb_hal_pic18.h"
#line 220 "./usb_hal_pic18.h"
#line 221 "./usb_hal_pic18.h"

#line 223 "./usb_hal_pic18.h"
#line 224 "./usb_hal_pic18.h"
#line 225 "./usb_hal_pic18.h"
#line 226 "./usb_hal_pic18.h"

#line 228 "./usb_hal_pic18.h"
#line 229 "./usb_hal_pic18.h"
#line 230 "./usb_hal_pic18.h"
#line 231 "./usb_hal_pic18.h"
#line 232 "./usb_hal_pic18.h"
#line 233 "./usb_hal_pic18.h"
#line 234 "./usb_hal_pic18.h"
#line 235 "./usb_hal_pic18.h"
#line 236 "./usb_hal_pic18.h"
#line 237 "./usb_hal_pic18.h"
#line 238 "./usb_hal_pic18.h"
#line 239 "./usb_hal_pic18.h"

 
#line 242 "./usb_hal_pic18.h"
#line 243 "./usb_hal_pic18.h"
#line 244 "./usb_hal_pic18.h"
#line 245 "./usb_hal_pic18.h"
#line 246 "./usb_hal_pic18.h"
#line 247 "./usb_hal_pic18.h"
#line 248 "./usb_hal_pic18.h"
#line 249 "./usb_hal_pic18.h"
#line 250 "./usb_hal_pic18.h"

#line 252 "./usb_hal_pic18.h"

 
typedef union _BD_STAT
{
    BYTE Val;
    struct{
        
        unsigned BC8:1;         
        unsigned BC9:1;         
        unsigned BSTALL:1;      
        unsigned DTSEN:1;       
        unsigned INCDIS:1;      
        unsigned KEN:1;         
        unsigned DTS:1;         
        unsigned UOWN:1;        
    };
    struct{
        
        
        unsigned BC8:1;         
        unsigned BC9:1;         
        unsigned PID0:1;        
        unsigned PID1:1;
        unsigned PID2:1;
        unsigned PID3:1;
        unsigned :1;
        unsigned UOWN:1;        
    };
    struct{
        unsigned :2;
        unsigned PID:4;         
        unsigned :2;
    };
} BD_STAT;                      


typedef union __BDT
{
    struct
    {
        BD_STAT STAT;
        BYTE CNT;
        BYTE ADRL;                      
        BYTE ADRH;                      
    };
    struct
    {
        unsigned :8;
        unsigned :8;
        BYTE* ADR;                      
    };
    DWORD Val;
    BYTE v[4];
} BDT_ENTRY;

#line 308 "./usb_hal_pic18.h"
#line 309 "./usb_hal_pic18.h"
#line 310 "./usb_hal_pic18.h"
#line 311 "./usb_hal_pic18.h"
#line 312 "./usb_hal_pic18.h"

#line 314 "./usb_hal_pic18.h"
#line 315 "./usb_hal_pic18.h"
#line 316 "./usb_hal_pic18.h"

typedef union
{
    WORD UEP[16];
} _UEP;

#line 323 "./usb_hal_pic18.h"

#line 325 "./usb_hal_pic18.h"
#line 326 "./usb_hal_pic18.h"
#line 328 "./usb_hal_pic18.h"
#line 329 "./usb_hal_pic18.h"
#line 330 "./usb_hal_pic18.h"

typedef union _POINTER
{
    struct
    {
        BYTE bLow;
        BYTE bHigh;
        
    };
    WORD _word;                         
    
    

    BYTE* bRam;                         
                                        
    WORD* wRam;                         
                                        

    ROM BYTE* bRom;                     
    ROM WORD* wRom;
    
    
    
    
} POINTER;

#line 357 "./usb_hal_pic18.h"
#line 358 "./usb_hal_pic18.h"
#line 359 "./usb_hal_pic18.h"
#line 360 "./usb_hal_pic18.h"

    
#line 363 "./usb_hal_pic18.h"
#line 364 "./usb_hal_pic18.h"
#line 365 "./usb_hal_pic18.h"
#line 366 "./usb_hal_pic18.h"
#line 367 "./usb_hal_pic18.h"
#line 368 "./usb_hal_pic18.h"
    

#line 371 "./usb_hal_pic18.h"
#line 372 "./usb_hal_pic18.h"
    
#line 374 "./usb_hal_pic18.h"
#line 375 "./usb_hal_pic18.h"
    
#line 377 "./usb_hal_pic18.h"
#line 378 "./usb_hal_pic18.h"

#line 380 "./usb_hal_pic18.h"
#line 381 "./usb_hal_pic18.h"
#line 632 "./usb_hal.h"

#line 634 "./usb_hal.h"
#line 635 "./usb_hal.h"

#line 637 "./usb_hal.h"
 

#line 116 "./usb.h"
            

#line 119 "./usb.h"
#line 1 "./usb_device.h"

#line 48 "./usb_device.h"
 


#line 92 "./usb_device.h"
 


#line 96 "./usb_device.h"



#line 1 "./Compiler.h"

#line 51 "./Compiler.h"
 

#line 54 "./Compiler.h"

					
#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"

#line 3 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"

#line 5 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 7 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 9 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 11 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 13 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 15 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 17 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 19 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 21 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 23 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 25 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 27 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 29 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 31 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 33 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 35 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 37 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 41 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 43 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 45 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 47 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 49 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 51 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 53 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 55 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 57 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 59 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 61 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 63 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 65 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 67 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 69 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 71 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 73 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 75 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 77 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 79 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 81 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 83 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 85 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 87 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 89 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 91 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 93 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 95 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 97 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 99 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 101 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 103 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 105 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 107 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 109 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 111 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 113 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 115 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 117 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 119 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 121 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 123 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 125 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 127 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 129 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 131 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 133 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 135 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 137 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 139 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 141 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 143 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 145 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 147 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 149 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 151 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 153 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 155 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 157 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 159 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 161 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 163 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 165 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 167 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 169 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 171 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 173 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 175 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 177 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 179 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 181 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 183 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 185 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 187 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 189 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 191 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 193 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 195 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 197 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 199 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 201 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 203 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 205 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 207 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 209 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 211 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 213 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 215 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 217 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 219 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 221 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 223 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 225 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 227 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 229 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 231 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 233 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 235 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 237 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 239 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 241 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 243 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 245 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 247 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 249 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 251 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 253 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 255 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 257 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 259 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 261 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 263 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 265 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 267 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 269 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 271 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 273 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 275 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 277 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 279 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 281 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 283 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 285 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 287 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 289 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 291 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 293 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 295 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 297 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 299 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 301 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 303 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 305 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 307 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 309 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 311 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 313 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 315 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 317 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 319 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 321 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 323 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 325 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 327 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 329 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 331 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 333 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 335 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 337 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 339 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 341 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 343 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 345 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 347 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 349 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 351 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 353 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 355 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 357 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 359 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 361 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 363 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 365 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 367 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 369 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 371 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 373 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 375 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 377 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 379 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 381 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 383 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 385 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 387 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 389 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 391 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 393 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 395 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 397 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 399 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 401 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 403 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 405 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 407 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 409 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 411 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"

#line 33 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
 


#line 37 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"

extern volatile far  unsigned            PMSTAT;
extern volatile far  unsigned char       PMSTATL;
extern volatile far  struct {
  unsigned OB0E:1;
  unsigned OB1E:1;
  unsigned OB2E:1;
  unsigned OB3E:1;
  unsigned :2;
  unsigned OBUF:1;
  unsigned OBE:1;
} PMSTATLbits;
extern volatile far  unsigned char       PMSTATH;
extern volatile far  struct {
  unsigned IB0F:1;
  unsigned IB1F:1;
  unsigned IB2F:1;
  unsigned IB3F:1;
  unsigned :2;
  unsigned IBOV:1;
  unsigned IBF:1;
} PMSTATHbits;
extern volatile far  unsigned char       PMEL;
extern volatile far  union {
  struct {
    unsigned PTENL:8;
  };
  struct {
    unsigned PTEN0:1;
    unsigned PTEN1:1;
    unsigned PTEN2:1;
    unsigned PTEN3:1;
    unsigned PTEN4:1;
    unsigned PTEN5:1;
    unsigned PTEN6:1;
    unsigned PTEN7:1;
  };
} PMELbits;
extern volatile far  unsigned            PMEN;
extern volatile far  unsigned char       PMEH;
extern volatile far  union {
  struct {
    unsigned PTENH:8;
  };
  struct {
    unsigned PTEN8:1;
    unsigned PTEN9:1;
    unsigned PTEN10:1;
    unsigned PTEN11:1;
    unsigned PTEN12:1;
    unsigned PTEN13:1;
    unsigned PTEN14:1;
    unsigned PTEN15:1;
  };
} PMEHbits;
extern volatile far  unsigned            PMDIN2;
extern volatile far  unsigned char       PMDIN2L;
extern volatile far  unsigned char       PMDIN2H;
extern volatile far  unsigned            PMDOUT2;
extern volatile far  unsigned char       PMDOUT2L;
extern volatile far  unsigned char       PMDOUT2H;
extern volatile far  unsigned            PMMODE;
extern volatile far  unsigned char       PMMODEL;
extern volatile far  union {
  struct {
    unsigned WAITE:2;
    unsigned WAITM:4;
    unsigned WAITB:2;
  };
  struct {
    unsigned WAITE0:1;
    unsigned WAITE1:1;
    unsigned WAITM0:1;
    unsigned WAITM1:1;
    unsigned WAITM2:1;
    unsigned WAITM3:1;
    unsigned WAITB0:1;
    unsigned WAITB1:1;
  };
} PMMODELbits;
extern volatile far  unsigned char       PMMODEH;
extern volatile far  union {
  struct {
    unsigned MODE:2;
    unsigned MODE16:1;
    unsigned INCM:2;
    unsigned IRQM:2;
    unsigned BUSY:1;
  };
  struct {
    unsigned MODE0:1;
    unsigned MODE1:1;
    unsigned :1;
    unsigned INCM0:1;
    unsigned INCM1:1;
    unsigned IRQM0:1;
    unsigned IRQM1:1;
  };
} PMMODEHbits;
extern volatile far  unsigned            PMCON;
extern volatile far  unsigned char       PMCONL;
extern volatile far  struct {
  unsigned RDSP:1;
  unsigned WRSP:1;
  unsigned BEP:1;
  unsigned CS1P:1;
  unsigned CS2P:1;
  unsigned ALP:1;
  unsigned CSF0:1;
  unsigned CSF1:1;
} PMCONLbits;
extern volatile far  unsigned char       PMCONH;
extern volatile far  struct {
  unsigned PTRDEN:1;
  unsigned PTWREN:1;
  unsigned PTBEEN:1;
  unsigned ADRMUX0:1;
  unsigned ADRMUX1:1;
  unsigned PSIDL:1;
  unsigned :1;
  unsigned PMPEN:1;
} PMCONHbits;
extern volatile far  unsigned char       UEP0;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP0bits;
extern volatile far  unsigned char       UEP1;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP1bits;
extern volatile far  unsigned char       UEP2;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP2bits;
extern volatile far  unsigned char       UEP3;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP3bits;
extern volatile far  unsigned char       UEP4;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP4bits;
extern volatile far  unsigned char       UEP5;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP5bits;
extern volatile far  unsigned char       UEP6;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP6bits;
extern volatile far  unsigned char       UEP7;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP7bits;
extern volatile far  unsigned char       UEP8;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP8bits;
extern volatile far  unsigned char       UEP9;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP9bits;
extern volatile far  unsigned char       UEP10;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP10bits;
extern volatile far  unsigned char       UEP11;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP11bits;
extern volatile far  unsigned char       UEP12;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP12bits;
extern volatile far  unsigned char       UEP13;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP13bits;
extern volatile far  unsigned char       UEP14;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP14bits;
extern volatile far  unsigned char       UEP15;
extern volatile far  struct {
  unsigned EPSTALL:1;
  unsigned EPINEN:1;
  unsigned EPOUTEN:1;
  unsigned EPCONDIS:1;
  unsigned EPHSHK:1;
} UEP15bits;
extern volatile far  unsigned char       UIE;
extern volatile far  struct {
  unsigned URSTIE:1;
  unsigned UERRIE:1;
  unsigned ACTVIE:1;
  unsigned TRNIE:1;
  unsigned IDLEIE:1;
  unsigned STALLIE:1;
  unsigned SOFIE:1;
} UIEbits;
extern volatile far  unsigned char       UEIE;
extern volatile far  struct {
  unsigned PIDEE:1;
  unsigned CRC5EE:1;
  unsigned CRC16EE:1;
  unsigned DFN8EE:1;
  unsigned BTOEE:1;
  unsigned :2;
  unsigned BTSEE:1;
} UEIEbits;
extern volatile far  unsigned char       UADDR;
extern volatile far  union {
  struct {
    unsigned ADDR:7;
  };
  struct {
    unsigned ADDR0:1;
    unsigned ADDR1:1;
    unsigned ADDR2:1;
    unsigned ADDR3:1;
    unsigned ADDR4:1;
    unsigned ADDR5:1;
    unsigned ADDR6:1;
  };
} UADDRbits;
extern volatile far  unsigned char       UCFG;
extern volatile far  union {
  struct {
    unsigned PPB:2;
    unsigned FSEN:1;
    unsigned UTRDIS:1;
    unsigned UPUEN:1;
    unsigned :1;
    unsigned UOEMON:1;
    unsigned UTEYE:1;
  };
  struct {
    unsigned PPB0:1;
    unsigned PPB1:1;
  };
} UCFGbits;
extern volatile near unsigned            UFRM;
extern volatile near unsigned char       UFRML;
extern volatile near union {
  struct {
    unsigned FRM:8;
  };
  struct {
    unsigned FRM0:1;
    unsigned FRM1:1;
    unsigned FRM2:1;
    unsigned FRM3:1;
    unsigned FRM4:1;
    unsigned FRM5:1;
    unsigned FRM6:1;
    unsigned FRM7:1;
  };
} UFRMLbits;
extern volatile near unsigned char       UFRMH;
extern volatile near union {
  struct {
    unsigned FRM:3;
  };
  struct {
    unsigned FRM8:1;
    unsigned FRM9:1;
    unsigned FRM10:1;
  };
} UFRMHbits;
extern volatile near unsigned char       UIR;
extern volatile near struct {
  unsigned URSTIF:1;
  unsigned UERRIF:1;
  unsigned ACTVIF:1;
  unsigned TRNIF:1;
  unsigned IDLEIF:1;
  unsigned STALLIF:1;
  unsigned SOFIF:1;
} UIRbits;
extern volatile near unsigned char       UEIR;
extern volatile near struct {
  unsigned PIDEF:1;
  unsigned CRC5EF:1;
  unsigned CRC16EF:1;
  unsigned DFN8EF:1;
  unsigned BTOEF:1;
  unsigned :2;
  unsigned BTSEF:1;
} UEIRbits;
extern volatile near unsigned char       USTAT;
extern volatile near union {
  struct {
    unsigned :1;
    unsigned PPBI:1;
    unsigned DIR:1;
    unsigned ENDP:4;
  };
  struct {
    unsigned :3;
    unsigned ENDP0:1;
    unsigned ENDP1:1;
    unsigned ENDP2:1;
    unsigned ENDP3:1;
  };
} USTATbits;
extern volatile near unsigned char       UCON;
extern volatile near struct {
  unsigned :1;
  unsigned SUSPND:1;
  unsigned RESUME:1;
  unsigned USBEN:1;
  unsigned PKTDIS:1;
  unsigned SE0:1;
  unsigned PPBRST:1;
} UCONbits;
extern volatile near unsigned            PMDIN1;
extern volatile near unsigned char       PMDIN1L;
extern volatile near unsigned char       PMDIN1H;
extern volatile near unsigned            PMADDR;
extern volatile near unsigned char       PMADDRL;
extern volatile near unsigned            PMDOUT1;
extern volatile near unsigned char       PMDOUT1L;
extern volatile near unsigned char       PMADDRH;
extern volatile near struct {
  unsigned ADDRH:6;
  unsigned CS1:1;
  unsigned CS2:1;
} PMADDRHbits;
extern volatile near unsigned char       PMDOUT1H;
extern volatile near unsigned char       CMSTAT;
extern volatile near struct {
  unsigned COUT1:1;
  unsigned COUT2:1;
} CMSTATbits;
extern volatile near unsigned char       CMSTATUS;
extern volatile near struct {
  unsigned COUT1:1;
  unsigned COUT2:1;
} CMSTATUSbits;
extern volatile near unsigned char       SSP2CON2;
extern volatile near union {
  struct {
    unsigned SEN:1;
    unsigned RSEN:1;
    unsigned PEN:1;
    unsigned RCEN:1;
    unsigned ACKEN:1;
    unsigned ACKDT:1;
    unsigned ACKSTAT:1;
    unsigned GCEN:1;
  };
  struct {
    unsigned :1;
    unsigned ADMSK1:1;
    unsigned ADMSK2:1;
    unsigned ADMSK3:1;
    unsigned ADMSK4:1;
    unsigned ADMSK5:1;
  };
} SSP2CON2bits;
extern volatile near unsigned char       SSP2CON1;
extern volatile near union {
  struct {
    unsigned SSPM:4;
    unsigned CKP:1;
    unsigned SSPEN:1;
    unsigned SSPOV:1;
    unsigned WCOL:1;
  };
  struct {
    unsigned SSPM0:1;
    unsigned SSPM1:1;
    unsigned SSPM2:1;
    unsigned SSPM3:1;
  };
} SSP2CON1bits;
extern volatile near unsigned char       SSP2STAT;
extern volatile near union {
  struct {
    unsigned BF:1;
    unsigned UA:1;
    unsigned R_NOT_W:1;
    unsigned S:1;
    unsigned P:1;
    unsigned D_NOT_A:1;
    unsigned CKE:1;
    unsigned SMP:1;
  };
  struct {
    unsigned :2;
    unsigned R_W:1;
    unsigned :2;
    unsigned D_A:1;
  };
  struct {
    unsigned :2;
    unsigned I2C_READ:1;
    unsigned I2C_START:1;
    unsigned I2C_STOP:1;
    unsigned I2C_DAT:1;
  };
  struct {
    unsigned :2;
    unsigned NOT_W:1;
    unsigned :2;
    unsigned NOT_A:1;
  };
  struct {
    unsigned :2;
    unsigned NOT_WRITE:1;
    unsigned :2;
    unsigned NOT_ADDRESS:1;
  };
  struct {
    unsigned :2;
    unsigned READ_WRITE:1;
    unsigned :2;
    unsigned DATA_ADDRESS:1;
  };
  struct {
    unsigned :2;
    unsigned R:1;
    unsigned :2;
    unsigned D:1;
  };
} SSP2STATbits;
extern volatile near unsigned char       SSP2ADD;
extern volatile near unsigned char       SSP2MSK;
extern volatile near struct {
  unsigned MSK0:1;
  unsigned MSK1:1;
  unsigned MSK2:1;
  unsigned MSK3:1;
  unsigned MSK4:1;
  unsigned MSK5:1;
  unsigned MSK6:1;
  unsigned MSK7:1;
} SSP2MSKbits;
extern volatile near unsigned char       SSP2BUF;
extern volatile near unsigned char       CCP5CON;
extern volatile near union {
  struct {
    unsigned CCP5M:4;
    unsigned DC5B:2;
  };
  struct {
    unsigned CCP5M0:1;
    unsigned CCP5M1:1;
    unsigned CCP5M2:1;
    unsigned CCP5M3:1;
    unsigned DC5B0:1;
    unsigned DC5B1:1;
  };
  struct {
    unsigned :4;
    unsigned DCCP5Y:1;
    unsigned DCCP5X:1;
  };
  struct {
    unsigned :4;
    unsigned DC5Y:1;
    unsigned DC5X:1;
  };
} CCP5CONbits;
extern volatile near unsigned            CCPR5;
extern volatile near unsigned char       CCPR5L;
extern volatile near unsigned char       CCPR5H;
extern volatile near unsigned char       CCP4CON;
extern volatile near union {
  struct {
    unsigned CCP4M:4;
    unsigned DC4B:2;
  };
  struct {
    unsigned CCP4M0:1;
    unsigned CCP4M1:1;
    unsigned CCP4M2:1;
    unsigned CCP4M3:1;
    unsigned DC4B0:1;
    unsigned DC4B1:1;
  };
  struct {
    unsigned :4;
    unsigned DCCP4Y:1;
    unsigned DCCP4X:1;
  };
  struct {
    unsigned :4;
    unsigned DC4Y:1;
    unsigned DC4X:1;
  };
} CCP4CONbits;
extern volatile near unsigned            CCPR4;
extern volatile near unsigned char       CCPR4L;
extern volatile near unsigned char       CCPR4H;
extern volatile near unsigned char       T4CON;
extern volatile near union {
  struct {
    unsigned T4CKPS:2;
    unsigned TMR4ON:1;
    unsigned T4OUTPS:4;
  };
  struct {
    unsigned T4CKPS0:1;
    unsigned T4CKPS1:1;
    unsigned :1;
    unsigned T4OUTPS0:1;
    unsigned T4OUTPS1:1;
    unsigned T4OUTPS2:1;
    unsigned T4OUTPS3:1;
  };
} T4CONbits;
extern volatile near unsigned char       CVRCON;
extern volatile near union {
  struct {
    unsigned CVR:4;
    unsigned CVRSS:1;
    unsigned CVRR:1;
    unsigned CVROE:1;
    unsigned CVREN:1;
  };
  struct {
    unsigned CVR0:1;
    unsigned CVR1:1;
    unsigned CVR2:1;
    unsigned CVR3:1;
  };
} CVRCONbits;
extern volatile near unsigned char       PR4;
extern volatile near unsigned char       TMR4;
extern volatile near unsigned char       T3CON;
extern volatile near union {
  struct {
    unsigned TMR3ON:1;
    unsigned TMR3CS:1;
    unsigned NOT_T3SYNC:1;
    unsigned T3CCP1:1;
    unsigned T3CKPS:2;
    unsigned T3CCP2:1;
    unsigned RD16:1;
  };
  struct {
    unsigned :2;
    unsigned T3SYNC:1;
    unsigned :1;
    unsigned T3CKPS0:1;
    unsigned T3CKPS1:1;
  };
  struct {
    unsigned :2;
    unsigned T3INSYNC:1;
  };
} T3CONbits;
extern volatile near unsigned char       TMR3L;
extern volatile near unsigned char       TMR3H;
extern volatile near unsigned char       BAUDCON2;
extern volatile near union {
  struct {
    unsigned ABDEN:1;
    unsigned WUE:1;
    unsigned :1;
    unsigned BRG16:1;
    unsigned TXCKP:1;
    unsigned RXDTP:1;
    unsigned RCIDL:1;
    unsigned ABDOVF:1;
  };
  struct {
    unsigned :4;
    unsigned SCKP:1;
    unsigned DTRXP:1;
    unsigned RCMT:1;
  };
} BAUDCON2bits;
extern volatile near unsigned char       SPBRGH2;
extern volatile near unsigned char       BAUDCON;
extern volatile near union {
  struct {
    unsigned ABDEN:1;
    unsigned WUE:1;
    unsigned :1;
    unsigned BRG16:1;
    unsigned TXCKP:1;
    unsigned RXDTP:1;
    unsigned RCIDL:1;
    unsigned ABDOVF:1;
  };
  struct {
    unsigned :4;
    unsigned SCKP:1;
    unsigned DTRXP:1;
    unsigned RCMT:1;
  };
} BAUDCONbits;
extern volatile near unsigned char       BAUDCON1;
extern volatile near union {
  struct {
    unsigned ABDEN:1;
    unsigned WUE:1;
    unsigned :1;
    unsigned BRG16:1;
    unsigned TXCKP:1;
    unsigned RXDTP:1;
    unsigned RCIDL:1;
    unsigned ABDOVF:1;
  };
  struct {
    unsigned :4;
    unsigned SCKP:1;
    unsigned DTRXP:1;
    unsigned RCMT:1;
  };
} BAUDCON1bits;
extern volatile near unsigned char       SPBRGH;
extern volatile near unsigned char       SPBRGH1;
extern volatile near unsigned char       PORTA;
extern volatile near union {
  struct {
    unsigned RA0:1;
    unsigned RA1:1;
    unsigned RA2:1;
    unsigned RA3:1;
    unsigned RA4:1;
    unsigned RA5:1;
    unsigned RA6:1;
    unsigned RA7:1;
  };
  struct {
    unsigned AN0:1;
    unsigned AN1:1;
    unsigned AN2:1;
    unsigned AN3:1;
    unsigned T0CKI:1;
    unsigned AN4:1;
    unsigned OSC2:1;
  };
  struct {
    unsigned :2;
    unsigned VREFM:1;
    unsigned VREFP:1;
    unsigned :1;
    unsigned C2INA:1;
    unsigned CLKO:1;
  };
  struct {
    unsigned :4;
    unsigned PMD5:1;
    unsigned PMD4:1;
  };
} PORTAbits;
extern volatile near unsigned char       PORTB;
extern volatile near union {
  struct {
    unsigned RB0:1;
    unsigned RB1:1;
    unsigned RB2:1;
    unsigned RB3:1;
    unsigned RB4:1;
    unsigned RB5:1;
    unsigned RB6:1;
    unsigned RB7:1;
  };
  struct {
    unsigned INT0:1;
    unsigned INT1:1;
    unsigned INT2:1;
    unsigned INT3:1;
    unsigned KBI0:1;
    unsigned KBI1:1;
    unsigned KBI2:1;
    unsigned KBI3:1;
  };
  struct {
    unsigned :1;
    unsigned PMA4:1;
    unsigned PMA3:1;
    unsigned PMA2:1;
    unsigned PMA1:1;
    unsigned PMA0:1;
  };
  struct {
    unsigned FLT0:1;
    unsigned :2;
    unsigned CCP2:1;
  };
  struct {
    unsigned :5;
    unsigned PGC:1;
    unsigned PGD:1;
  };
} PORTBbits;
extern volatile near unsigned char       PORTC;
extern volatile near union {
  struct {
    unsigned RC0:1;
    unsigned RC1:1;
    unsigned RC2:1;
    unsigned RC3:1;
    unsigned RC4:1;
    unsigned RC5:1;
    unsigned RC6:1;
    unsigned RC7:1;
  };
  struct {
    unsigned T1OSO:1;
    unsigned T1OSI:1;
    unsigned CCP1:1;
    unsigned SCK:1;
    unsigned SDI:1;
    unsigned SDO:1;
    unsigned TX:1;
    unsigned RX:1;
  };
  struct {
    unsigned T13CKI:1;
    unsigned CCP2:1;
    unsigned :1;
    unsigned SCL:1;
    unsigned SDA:1;
    unsigned :1;
    unsigned CK:1;
    unsigned DT:1;
  };
  struct {
    unsigned :5;
    unsigned C2OUT:1;
  };
} PORTCbits;
extern volatile near unsigned char       PORTD;
extern volatile near union {
  struct {
    unsigned RD0:1;
    unsigned RD1:1;
    unsigned RD2:1;
    unsigned RD3:1;
    unsigned RD4:1;
    unsigned RD5:1;
    unsigned RD6:1;
    unsigned RD7:1;
  };
  struct {
    unsigned PMD0:1;
    unsigned PMD1:1;
    unsigned PMD2:1;
    unsigned PMD3:1;
    unsigned PMD4:1;
    unsigned PMD5:1;
    unsigned PMD6:1;
    unsigned PMD7:1;
  };
  struct {
    unsigned AD0:1;
    unsigned AD1:1;
    unsigned AD2:1;
    unsigned AD3:1;
    unsigned AD4:1;
    unsigned AD5:1;
    unsigned AD6:1;
    unsigned AD7:1;
  };
  struct {
    unsigned :5;
    unsigned SDA2:1;
    unsigned SCL2:1;
    unsigned SS2:1;
  };
  struct {
    unsigned :4;
    unsigned SDO2:1;
    unsigned SDI2:1;
    unsigned SCK2:1;
  };
} PORTDbits;
extern volatile near unsigned char       PORTE;
extern volatile near union {
  struct {
    unsigned RE0:1;
    unsigned RE1:1;
    unsigned RE2:1;
    unsigned RE3:1;
    unsigned RE4:1;
    unsigned RE5:1;
    unsigned RE6:1;
    unsigned RE7:1;
  };
  struct {
    unsigned AD8:1;
    unsigned AD9:1;
    unsigned AD10:1;
    unsigned AD11:1;
    unsigned AD12:1;
    unsigned AD13:1;
    unsigned AD14:1;
    unsigned AD15:1;
  };
  struct {
    unsigned PMRD:1;
    unsigned PMWR:1;
    unsigned PMBE:1;
    unsigned PMA13:1;
    unsigned PMA12:1;
    unsigned PMA11:1;
    unsigned PMA10:1;
    unsigned PMA9:1;
  };
  struct {
    unsigned :3;
    unsigned REFO:1;
    unsigned :3;
    unsigned CCP2:1;
  };
} PORTEbits;
extern volatile near unsigned char       PORTF;
extern volatile near union {
  struct {
    unsigned :2;
    unsigned RF2:1;
    unsigned RF3:1;
    unsigned RF4:1;
    unsigned RF5:1;
    unsigned RF6:1;
    unsigned RF7:1;
  };
  struct {
    unsigned :2;
    unsigned AN7:1;
    unsigned :2;
    unsigned AN10:1;
    unsigned AN11:1;
    unsigned SS:1;
  };
  struct {
    unsigned :5;
    unsigned CVREF:1;
  };
  struct {
    unsigned :2;
    unsigned C2INB:1;
    unsigned :2;
    unsigned C1INB:1;
    unsigned C1INA:1;
    unsigned C1OUT:1;
  };
  struct {
    unsigned :2;
    unsigned PMA5:1;
    unsigned :2;
    unsigned PMD2:1;
    unsigned PMD1:1;
    unsigned PMD0:1;
  };
} PORTFbits;
extern volatile near unsigned char       PORTG;
extern volatile near union {
  struct {
    unsigned RG0:1;
    unsigned RG1:1;
    unsigned RG2:1;
    unsigned RG3:1;
    unsigned RG4:1;
    unsigned RJPU:1;
    unsigned REPU:1;
    unsigned RDPU:1;
  };
  struct {
    unsigned CCP3:1;
    unsigned TX2:1;
    unsigned RX2:1;
    unsigned CCP4:1;
    unsigned CCP5:1;
  };
  struct {
    unsigned :1;
    unsigned CK2:1;
    unsigned DT2:1;
  };
  struct {
    unsigned PMA8:1;
    unsigned PMA7:1;
    unsigned PMA6:1;
    unsigned PMCS1:1;
    unsigned PMCS2:1;
  };
} PORTGbits;
extern volatile near unsigned char       PORTH;
extern volatile near union {
  struct {
    unsigned RH0:1;
    unsigned RH1:1;
    unsigned RH2:1;
    unsigned RH3:1;
    unsigned RH4:1;
    unsigned RH5:1;
    unsigned RH6:1;
    unsigned RH7:1;
  };
  struct {
    unsigned A16:1;
    unsigned A17:1;
    unsigned A18:1;
    unsigned A19:1;
    unsigned AN12:1;
    unsigned AN13:1;
    unsigned AN14:1;
    unsigned AN15:1;
  };
  struct {
    unsigned :2;
    unsigned PMD7:1;
    unsigned PMD6:1;
    unsigned PMD3:1;
    unsigned PMBE:1;
    unsigned PMRD:1;
    unsigned PMWR:1;
  };
  struct {
    unsigned :4;
    unsigned C2INC:1;
    unsigned C2IND:1;
    unsigned C1INC:1;
  };
} PORTHbits;
extern volatile near unsigned char       PORTJ;
extern volatile near union {
  struct {
    unsigned RJ0:1;
    unsigned RJ1:1;
    unsigned RJ2:1;
    unsigned RJ3:1;
    unsigned RJ4:1;
    unsigned RJ5:1;
    unsigned RJ6:1;
    unsigned RJ7:1;
  };
  struct {
    unsigned ALE:1;
    unsigned OE:1;
    unsigned WRL:1;
    unsigned WRH:1;
    unsigned BA0:1;
    unsigned CE:1;
    unsigned LB:1;
    unsigned UB:1;
  };
} PORTJbits;
extern volatile near unsigned char       LATA;
extern volatile near struct {
  unsigned LATA0:1;
  unsigned LATA1:1;
  unsigned LATA2:1;
  unsigned LATA3:1;
  unsigned LATA4:1;
  unsigned LATA5:1;
  unsigned LATA6:1;
  unsigned LATA7:1;
} LATAbits;
extern volatile near unsigned char       LATB;
extern volatile near struct {
  unsigned LATB0:1;
  unsigned LATB1:1;
  unsigned LATB2:1;
  unsigned LATB3:1;
  unsigned LATB4:1;
  unsigned LATB5:1;
  unsigned LATB6:1;
  unsigned LATB7:1;
} LATBbits;
extern volatile near unsigned char       LATC;
extern volatile near struct {
  unsigned LATC0:1;
  unsigned LATC1:1;
  unsigned LATC2:1;
  unsigned LATC3:1;
  unsigned LATC4:1;
  unsigned LATC5:1;
  unsigned LATC6:1;
  unsigned LATC7:1;
} LATCbits;
extern volatile near unsigned char       LATD;
extern volatile near struct {
  unsigned LATD0:1;
  unsigned LATD1:1;
  unsigned LATD2:1;
  unsigned LATD3:1;
  unsigned LATD4:1;
  unsigned LATD5:1;
  unsigned LATD6:1;
  unsigned LATD7:1;
} LATDbits;
extern volatile near unsigned char       LATE;
extern volatile near struct {
  unsigned LATE0:1;
  unsigned LATE1:1;
  unsigned LATE2:1;
  unsigned LATE3:1;
  unsigned LATE4:1;
  unsigned LATE5:1;
  unsigned LATE6:1;
  unsigned LATE7:1;
} LATEbits;
extern volatile near unsigned char       LATF;
extern volatile near struct {
  unsigned :2;
  unsigned LATF2:1;
  unsigned LATF3:1;
  unsigned LATF4:1;
  unsigned LATF5:1;
  unsigned LATF6:1;
  unsigned LATF7:1;
} LATFbits;
extern volatile near unsigned char       LATG;
extern volatile near struct {
  unsigned LATG0:1;
  unsigned LATG1:1;
  unsigned LATG2:1;
  unsigned LATG3:1;
  unsigned LATG4:1;
} LATGbits;
extern volatile near unsigned char       LATH;
extern volatile near struct {
  unsigned LATH0:1;
  unsigned LATH1:1;
  unsigned LATH2:1;
  unsigned LATH3:1;
  unsigned LATH4:1;
  unsigned LATH5:1;
  unsigned LATH6:1;
  unsigned LATH7:1;
} LATHbits;
extern volatile near unsigned char       LATJ;
extern volatile near struct {
  unsigned LATJ0:1;
  unsigned LATJ1:1;
  unsigned LATJ2:1;
  unsigned LATJ3:1;
  unsigned LATJ4:1;
  unsigned LATJ5:1;
  unsigned LATJ6:1;
  unsigned LATJ7:1;
} LATJbits;
extern volatile near unsigned char       DDRA;
extern volatile near union {
  struct {
    unsigned TRISA0:1;
    unsigned TRISA1:1;
    unsigned TRISA2:1;
    unsigned TRISA3:1;
    unsigned TRISA4:1;
    unsigned TRISA5:1;
    unsigned TRISA6:1;
    unsigned TRISA7:1;
  };
  struct {
    unsigned RA0:1;
    unsigned RA1:1;
    unsigned RA2:1;
    unsigned RA3:1;
    unsigned RA4:1;
    unsigned RA5:1;
    unsigned RA6:1;
    unsigned RA7:1;
  };
} DDRAbits;
extern volatile near unsigned char       TRISA;
extern volatile near union {
  struct {
    unsigned TRISA0:1;
    unsigned TRISA1:1;
    unsigned TRISA2:1;
    unsigned TRISA3:1;
    unsigned TRISA4:1;
    unsigned TRISA5:1;
    unsigned TRISA6:1;
    unsigned TRISA7:1;
  };
  struct {
    unsigned RA0:1;
    unsigned RA1:1;
    unsigned RA2:1;
    unsigned RA3:1;
    unsigned RA4:1;
    unsigned RA5:1;
    unsigned RA6:1;
    unsigned RA7:1;
  };
} TRISAbits;
extern volatile near unsigned char       DDRB;
extern volatile near union {
  struct {
    unsigned TRISB0:1;
    unsigned TRISB1:1;
    unsigned TRISB2:1;
    unsigned TRISB3:1;
    unsigned TRISB4:1;
    unsigned TRISB5:1;
    unsigned TRISB6:1;
    unsigned TRISB7:1;
  };
  struct {
    unsigned RB0:1;
    unsigned RB1:1;
    unsigned RB2:1;
    unsigned RB3:1;
    unsigned RB4:1;
    unsigned RB5:1;
    unsigned RB6:1;
    unsigned RB7:1;
  };
} DDRBbits;
extern volatile near unsigned char       TRISB;
extern volatile near union {
  struct {
    unsigned TRISB0:1;
    unsigned TRISB1:1;
    unsigned TRISB2:1;
    unsigned TRISB3:1;
    unsigned TRISB4:1;
    unsigned TRISB5:1;
    unsigned TRISB6:1;
    unsigned TRISB7:1;
  };
  struct {
    unsigned RB0:1;
    unsigned RB1:1;
    unsigned RB2:1;
    unsigned RB3:1;
    unsigned RB4:1;
    unsigned RB5:1;
    unsigned RB6:1;
    unsigned RB7:1;
  };
} TRISBbits;
extern volatile near unsigned char       DDRC;
extern volatile near union {
  struct {
    unsigned TRISC0:1;
    unsigned TRISC1:1;
    unsigned TRISC2:1;
    unsigned TRISC3:1;
    unsigned TRISC4:1;
    unsigned TRISC5:1;
    unsigned TRISC6:1;
    unsigned TRISC7:1;
  };
  struct {
    unsigned RC0:1;
    unsigned RC1:1;
    unsigned RC2:1;
    unsigned RC3:1;
    unsigned RC4:1;
    unsigned RC5:1;
    unsigned RC6:1;
    unsigned RC7:1;
  };
} DDRCbits;
extern volatile near unsigned char       TRISC;
extern volatile near union {
  struct {
    unsigned TRISC0:1;
    unsigned TRISC1:1;
    unsigned TRISC2:1;
    unsigned TRISC3:1;
    unsigned TRISC4:1;
    unsigned TRISC5:1;
    unsigned TRISC6:1;
    unsigned TRISC7:1;
  };
  struct {
    unsigned RC0:1;
    unsigned RC1:1;
    unsigned RC2:1;
    unsigned RC3:1;
    unsigned RC4:1;
    unsigned RC5:1;
    unsigned RC6:1;
    unsigned RC7:1;
  };
} TRISCbits;
extern volatile near unsigned char       DDRD;
extern volatile near union {
  struct {
    unsigned TRISD0:1;
    unsigned TRISD1:1;
    unsigned TRISD2:1;
    unsigned TRISD3:1;
    unsigned TRISD4:1;
    unsigned TRISD5:1;
    unsigned TRISD6:1;
    unsigned TRISD7:1;
  };
  struct {
    unsigned RD0:1;
    unsigned RD1:1;
    unsigned RD2:1;
    unsigned RD3:1;
    unsigned RD4:1;
    unsigned RD5:1;
    unsigned RD6:1;
    unsigned RD7:1;
  };
} DDRDbits;
extern volatile near unsigned char       TRISD;
extern volatile near union {
  struct {
    unsigned TRISD0:1;
    unsigned TRISD1:1;
    unsigned TRISD2:1;
    unsigned TRISD3:1;
    unsigned TRISD4:1;
    unsigned TRISD5:1;
    unsigned TRISD6:1;
    unsigned TRISD7:1;
  };
  struct {
    unsigned RD0:1;
    unsigned RD1:1;
    unsigned RD2:1;
    unsigned RD3:1;
    unsigned RD4:1;
    unsigned RD5:1;
    unsigned RD6:1;
    unsigned RD7:1;
  };
} TRISDbits;
extern volatile near unsigned char       DDRE;
extern volatile near union {
  struct {
    unsigned TRISE0:1;
    unsigned TRISE1:1;
    unsigned TRISE2:1;
    unsigned TRISE3:1;
    unsigned TRISE4:1;
    unsigned TRISE5:1;
    unsigned TRISE6:1;
    unsigned TRISE7:1;
  };
  struct {
    unsigned RE0:1;
    unsigned RE1:1;
    unsigned RE2:1;
    unsigned RE3:1;
    unsigned RE4:1;
    unsigned RE5:1;
    unsigned RE6:1;
    unsigned RE7:1;
  };
} DDREbits;
extern volatile near unsigned char       TRISE;
extern volatile near union {
  struct {
    unsigned TRISE0:1;
    unsigned TRISE1:1;
    unsigned TRISE2:1;
    unsigned TRISE3:1;
    unsigned TRISE4:1;
    unsigned TRISE5:1;
    unsigned TRISE6:1;
    unsigned TRISE7:1;
  };
  struct {
    unsigned RE0:1;
    unsigned RE1:1;
    unsigned RE2:1;
    unsigned RE3:1;
    unsigned RE4:1;
    unsigned RE5:1;
    unsigned RE6:1;
    unsigned RE7:1;
  };
} TRISEbits;
extern volatile near unsigned char       DDRF;
extern volatile near union {
  struct {
    unsigned :2;
    unsigned TRISF2:1;
    unsigned TRISF3:1;
    unsigned TRISF4:1;
    unsigned TRISF5:1;
    unsigned TRISF6:1;
    unsigned TRISF7:1;
  };
  struct {
    unsigned :2;
    unsigned RF2:1;
    unsigned RF3:1;
    unsigned RF4:1;
    unsigned RF5:1;
    unsigned RF6:1;
    unsigned RF7:1;
  };
} DDRFbits;
extern volatile near unsigned char       TRISF;
extern volatile near union {
  struct {
    unsigned :2;
    unsigned TRISF2:1;
    unsigned TRISF3:1;
    unsigned TRISF4:1;
    unsigned TRISF5:1;
    unsigned TRISF6:1;
    unsigned TRISF7:1;
  };
  struct {
    unsigned :2;
    unsigned RF2:1;
    unsigned RF3:1;
    unsigned RF4:1;
    unsigned RF5:1;
    unsigned RF6:1;
    unsigned RF7:1;
  };
} TRISFbits;
extern volatile near unsigned char       DDRG;
extern volatile near union {
  struct {
    unsigned TRISG0:1;
    unsigned TRISG1:1;
    unsigned TRISG2:1;
    unsigned TRISG3:1;
    unsigned TRISG4:1;
  };
  struct {
    unsigned RG0:1;
    unsigned RG1:1;
    unsigned RG2:1;
    unsigned RG3:1;
    unsigned RG4:1;
  };
} DDRGbits;
extern volatile near unsigned char       TRISG;
extern volatile near union {
  struct {
    unsigned TRISG0:1;
    unsigned TRISG1:1;
    unsigned TRISG2:1;
    unsigned TRISG3:1;
    unsigned TRISG4:1;
  };
  struct {
    unsigned RG0:1;
    unsigned RG1:1;
    unsigned RG2:1;
    unsigned RG3:1;
    unsigned RG4:1;
  };
} TRISGbits;
extern volatile near unsigned char       DDRH;
extern volatile near union {
  struct {
    unsigned TRISH0:1;
    unsigned TRISH1:1;
    unsigned TRISH2:1;
    unsigned TRISH3:1;
    unsigned TRISH4:1;
    unsigned TRISH5:1;
    unsigned TRISH6:1;
    unsigned TRISH7:1;
  };
  struct {
    unsigned RH0:1;
    unsigned RH1:1;
    unsigned RH2:1;
    unsigned RH3:1;
    unsigned RH4:1;
    unsigned RH5:1;
    unsigned RH6:1;
    unsigned RH7:1;
  };
} DDRHbits;
extern volatile near unsigned char       TRISH;
extern volatile near union {
  struct {
    unsigned TRISH0:1;
    unsigned TRISH1:1;
    unsigned TRISH2:1;
    unsigned TRISH3:1;
    unsigned TRISH4:1;
    unsigned TRISH5:1;
    unsigned TRISH6:1;
    unsigned TRISH7:1;
  };
  struct {
    unsigned RH0:1;
    unsigned RH1:1;
    unsigned RH2:1;
    unsigned RH3:1;
    unsigned RH4:1;
    unsigned RH5:1;
    unsigned RH6:1;
    unsigned RH7:1;
  };
} TRISHbits;
extern volatile near unsigned char       DDRJ;
extern volatile near union {
  struct {
    unsigned TRISJ0:1;
    unsigned TRISJ1:1;
    unsigned TRISJ2:1;
    unsigned TRISJ3:1;
    unsigned TRISJ4:1;
    unsigned TRISJ5:1;
    unsigned TRISJ6:1;
    unsigned TRISJ7:1;
  };
  struct {
    unsigned RJ0:1;
    unsigned RJ1:1;
    unsigned RJ2:1;
    unsigned RJ3:1;
    unsigned RJ4:1;
    unsigned RJ5:1;
    unsigned RJ6:1;
    unsigned RJ7:1;
  };
} DDRJbits;
extern volatile near unsigned char       TRISJ;
extern volatile near union {
  struct {
    unsigned TRISJ0:1;
    unsigned TRISJ1:1;
    unsigned TRISJ2:1;
    unsigned TRISJ3:1;
    unsigned TRISJ4:1;
    unsigned TRISJ5:1;
    unsigned TRISJ6:1;
    unsigned TRISJ7:1;
  };
  struct {
    unsigned RJ0:1;
    unsigned RJ1:1;
    unsigned RJ2:1;
    unsigned RJ3:1;
    unsigned RJ4:1;
    unsigned RJ5:1;
    unsigned RJ6:1;
    unsigned RJ7:1;
  };
} TRISJbits;
extern volatile near unsigned char       OSCTUNE;
extern volatile near struct {
  unsigned TUN0:1;
  unsigned TUN1:1;
  unsigned TUN2:1;
  unsigned TUN3:1;
  unsigned TUN4:1;
  unsigned TUN5:1;
  unsigned PLLEN:1;
  unsigned INTSRC:1;
} OSCTUNEbits;
extern volatile near unsigned char       RCSTA2;
extern volatile near union {
  struct {
    unsigned RX9D:1;
    unsigned OERR:1;
    unsigned FERR:1;
    unsigned ADDEN:1;
    unsigned CREN:1;
    unsigned SREN:1;
    unsigned RX9:1;
    unsigned SPEN:1;
  };
  struct {
    unsigned RCD8:1;
    unsigned :5;
    unsigned RC9:1;
  };
  struct {
    unsigned :6;
    unsigned NOT_RC8:1;
  };
  struct {
    unsigned :6;
    unsigned RC8_9:1;
  };
  struct {
    unsigned RX9D2:1;
    unsigned OERR2:1;
    unsigned FERR2:1;
    unsigned ADDEN2:1;
    unsigned CREN2:1;
    unsigned SREN2:1;
    unsigned RX92:1;
    unsigned SPEN2:1;
  };
} RCSTA2bits;
extern volatile near unsigned char       PIE1;
extern volatile near union {
  struct {
    unsigned TMR1IE:1;
    unsigned TMR2IE:1;
    unsigned CCP1IE:1;
    unsigned SSP1IE:1;
    unsigned TX1IE:1;
    unsigned RC1IE:1;
    unsigned ADIE:1;
    unsigned PMPIE:1;
  };
  struct {
    unsigned :3;
    unsigned SSPIE:1;
    unsigned TXIE:1;
    unsigned RCIE:1;
  };
} PIE1bits;
extern volatile near unsigned char       PIR1;
extern volatile near union {
  struct {
    unsigned TMR1IF:1;
    unsigned TMR2IF:1;
    unsigned CCP1IF:1;
    unsigned SSP1IF:1;
    unsigned TX1IF:1;
    unsigned RC1IF:1;
    unsigned ADIF:1;
    unsigned PMPIF:1;
  };
  struct {
    unsigned :3;
    unsigned SSPIF:1;
    unsigned TXIF:1;
    unsigned RCIF:1;
  };
} PIR1bits;
extern volatile near unsigned char       IPR1;
extern volatile near union {
  struct {
    unsigned TMR1IP:1;
    unsigned TMR2IP:1;
    unsigned CCP1IP:1;
    unsigned SSP1IP:1;
    unsigned TX1IP:1;
    unsigned RC1IP:1;
    unsigned ADIP:1;
    unsigned PMPIP:1;
  };
  struct {
    unsigned :3;
    unsigned SSPIP:1;
    unsigned TXIP:1;
    unsigned RCIP:1;
  };
} IPR1bits;
extern volatile near unsigned char       PIE2;
extern volatile near union {
  struct {
    unsigned CCP2IE:1;
    unsigned TMR3IE:1;
    unsigned LVDIE:1;
    unsigned BCL1IE:1;
    unsigned USBIE:1;
    unsigned CM1IE:1;
    unsigned CM2IE:1;
    unsigned OSCFIE:1;
  };
  struct {
    unsigned :3;
    unsigned BCLIE:1;
  };
} PIE2bits;
extern volatile near unsigned char       PIR2;
extern volatile near union {
  struct {
    unsigned CCP2IF:1;
    unsigned TMR3IF:1;
    unsigned LVDIF:1;
    unsigned BCL1IF:1;
    unsigned USBIF:1;
    unsigned CM1IF:1;
    unsigned CM2IF:1;
    unsigned OSCFIF:1;
  };
  struct {
    unsigned :3;
    unsigned BCLIF:1;
  };
} PIR2bits;
extern volatile near unsigned char       IPR2;
extern volatile near union {
  struct {
    unsigned CCP2IP:1;
    unsigned TMR3IP:1;
    unsigned LVDIP:1;
    unsigned BCL1IP:1;
    unsigned USBIP:1;
    unsigned CM1IP:1;
    unsigned CM2IP:1;
    unsigned OSCFIP:1;
  };
  struct {
    unsigned :3;
    unsigned BCLIP:1;
  };
} IPR2bits;
extern volatile near unsigned char       PIE3;
extern volatile near struct {
  unsigned CCP3IE:1;
  unsigned CCP4IE:1;
  unsigned CCP5IE:1;
  unsigned TMR4IE:1;
  unsigned TX2IE:1;
  unsigned RC2IE:1;
  unsigned BCL2IE:1;
  unsigned SSP2IE:1;
} PIE3bits;
extern volatile near unsigned char       PIR3;
extern volatile near struct {
  unsigned CCP3IF:1;
  unsigned CCP4IF:1;
  unsigned CCP5IF:1;
  unsigned TMR4IF:1;
  unsigned TX2IF:1;
  unsigned RC2IF:1;
  unsigned BCL2IF:1;
  unsigned SSP2IF:1;
} PIR3bits;
extern volatile near unsigned char       IPR3;
extern volatile near struct {
  unsigned CCP3IP:1;
  unsigned CCP4IP:1;
  unsigned CCP5IP:1;
  unsigned TMR4IP:1;
  unsigned TX2IP:1;
  unsigned RC2IP:1;
  unsigned BCL2IP:1;
  unsigned SSP2IP:1;
} IPR3bits;
extern volatile near unsigned char       EECON1;
extern volatile near struct {
  unsigned :1;
  unsigned WR:1;
  unsigned WREN:1;
  unsigned WRERR:1;
  unsigned FREE:1;
  unsigned WPROG:1;
} EECON1bits;
extern volatile near unsigned char       EECON2;
extern volatile near unsigned char       TXSTA2;
extern volatile near union {
  struct {
    unsigned TX9D:1;
    unsigned TRMT:1;
    unsigned BRGH:1;
    unsigned SENDB:1;
    unsigned SYNC:1;
    unsigned TXEN:1;
    unsigned TX9:1;
    unsigned CSRC:1;
  };
  struct {
    unsigned TXD8:1;
    unsigned :5;
    unsigned TX8_9:1;
  };
  struct {
    unsigned :6;
    unsigned NOT_TX8:1;
  };
  struct {
    unsigned TX9D2:1;
    unsigned TRMT2:1;
    unsigned BRGH2:1;
    unsigned SENDB2:1;
    unsigned SYNC2:1;
    unsigned TXEN2:1;
    unsigned TX92:1;
    unsigned CSRC2:1;
  };
} TXSTA2bits;
extern volatile near unsigned char       TXREG2;
extern volatile near unsigned char       RCREG2;
extern volatile near unsigned char       SPBRG2;
extern volatile near unsigned char       RCSTA;
extern volatile near union {
  struct {
    unsigned RX9D:1;
    unsigned OERR:1;
    unsigned FERR:1;
    unsigned ADDEN:1;
    unsigned CREN:1;
    unsigned SREN:1;
    unsigned RX9:1;
    unsigned SPEN:1;
  };
  struct {
    unsigned RCD8:1;
    unsigned :5;
    unsigned RC9:1;
  };
  struct {
    unsigned :6;
    unsigned NOT_RC8:1;
  };
  struct {
    unsigned :6;
    unsigned RC8_9:1;
  };
  struct {
    unsigned RX9D1:1;
    unsigned OERR1:1;
    unsigned FERR1:1;
    unsigned ADDEN1:1;
    unsigned CREN1:1;
    unsigned SREN1:1;
    unsigned RX91:1;
    unsigned SPEN1:1;
  };
} RCSTAbits;
extern volatile near unsigned char       RCSTA1;
extern volatile near union {
  struct {
    unsigned RX9D:1;
    unsigned OERR:1;
    unsigned FERR:1;
    unsigned ADDEN:1;
    unsigned CREN:1;
    unsigned SREN:1;
    unsigned RX9:1;
    unsigned SPEN:1;
  };
  struct {
    unsigned RCD8:1;
    unsigned :5;
    unsigned RC9:1;
  };
  struct {
    unsigned :6;
    unsigned NOT_RC8:1;
  };
  struct {
    unsigned :6;
    unsigned RC8_9:1;
  };
  struct {
    unsigned RX9D1:1;
    unsigned OERR1:1;
    unsigned FERR1:1;
    unsigned ADDEN1:1;
    unsigned CREN1:1;
    unsigned SREN1:1;
    unsigned RX91:1;
    unsigned SPEN1:1;
  };
} RCSTA1bits;
extern volatile near unsigned char       TXSTA;
extern volatile near union {
  struct {
    unsigned TX9D:1;
    unsigned TRMT:1;
    unsigned BRGH:1;
    unsigned SENDB:1;
    unsigned SYNC:1;
    unsigned TXEN:1;
    unsigned TX9:1;
    unsigned CSRC:1;
  };
  struct {
    unsigned TXD8:1;
    unsigned :5;
    unsigned TX8_9:1;
  };
  struct {
    unsigned :6;
    unsigned NOT_TX8:1;
  };
  struct {
    unsigned TX9D1:1;
    unsigned TRMT1:1;
    unsigned BRGH1:1;
    unsigned SENDB1:1;
    unsigned SYNC1:1;
    unsigned TXEN1:1;
    unsigned TX91:1;
    unsigned CSRC1:1;
  };
} TXSTAbits;
extern volatile near unsigned char       TXSTA1;
extern volatile near union {
  struct {
    unsigned TX9D:1;
    unsigned TRMT:1;
    unsigned BRGH:1;
    unsigned SENDB:1;
    unsigned SYNC:1;
    unsigned TXEN:1;
    unsigned TX9:1;
    unsigned CSRC:1;
  };
  struct {
    unsigned TXD8:1;
    unsigned :5;
    unsigned TX8_9:1;
  };
  struct {
    unsigned :6;
    unsigned NOT_TX8:1;
  };
  struct {
    unsigned TX9D1:1;
    unsigned TRMT1:1;
    unsigned BRGH1:1;
    unsigned SENDB1:1;
    unsigned SYNC1:1;
    unsigned TXEN1:1;
    unsigned TX91:1;
    unsigned CSRC1:1;
  };
} TXSTA1bits;
extern volatile near unsigned char       TXREG;
extern volatile near unsigned char       TXREG1;
extern volatile near unsigned char       RCREG;
extern volatile near unsigned char       RCREG1;
extern volatile near unsigned char       SPBRG;
extern volatile near unsigned char       SPBRG1;
extern volatile near unsigned char       CCP3CON;
extern volatile near union {
  struct {
    unsigned CCP3M:4;
    unsigned DC3B:2;
    unsigned P3M:2;
  };
  struct {
    unsigned CCP3M0:1;
    unsigned CCP3M1:1;
    unsigned CCP3M2:1;
    unsigned CCP3M3:1;
    unsigned DC3B0:1;
    unsigned DC3B1:1;
    unsigned P3M0:1;
    unsigned P3M1:1;
  };
  struct {
    unsigned :4;
    unsigned CCP3Y:1;
    unsigned CCP3X:1;
  };
} CCP3CONbits;
extern volatile near unsigned char       ECCP3CON;
extern volatile near union {
  struct {
    unsigned CCP3M:4;
    unsigned DC3B:2;
    unsigned P3M:2;
  };
  struct {
    unsigned CCP3M0:1;
    unsigned CCP3M1:1;
    unsigned CCP3M2:1;
    unsigned CCP3M3:1;
    unsigned DC3B0:1;
    unsigned DC3B1:1;
    unsigned P3M0:1;
    unsigned P3M1:1;
  };
  struct {
    unsigned :4;
    unsigned CCP3Y:1;
    unsigned CCP3X:1;
  };
} ECCP3CONbits;
extern volatile near unsigned            CCPR3;
extern volatile near unsigned char       CCPR3L;
extern volatile near unsigned char       CCPR3H;
extern volatile near unsigned char       ECCP3DEL;
extern volatile near union {
  struct {
    unsigned PDC:7;
    unsigned PRSEN:1;
  };
  struct {
    unsigned PDC0:1;
    unsigned PDC1:1;
    unsigned PDC2:1;
    unsigned PDC3:1;
    unsigned PDC4:1;
    unsigned PDC5:1;
    unsigned PDC6:1;
  };
  struct {
    unsigned P3DC0:1;
    unsigned P3DC1:1;
    unsigned P3DC2:1;
    unsigned P3DC3:1;
    unsigned P3DC4:1;
    unsigned P3DC5:1;
    unsigned P3DC6:1;
    unsigned P3RSEN:1;
  };
} ECCP3DELbits;
extern volatile near unsigned char       ECCP3AS;
extern volatile near union {
  struct {
    unsigned PSSBD:2;
    unsigned PSSAC:2;
    unsigned ECCPAS:3;
    unsigned ECCPASE:1;
  };
  struct {
    unsigned PSSBD0:1;
    unsigned PSSBD1:1;
    unsigned PSSAC0:1;
    unsigned PSSAC1:1;
    unsigned ECCPAS0:1;
    unsigned ECCPAS1:1;
    unsigned ECCPAS2:1;
  };
  struct {
    unsigned PSS3BD0:1;
    unsigned PSS3BD1:1;
    unsigned PSS3AC0:1;
    unsigned PSS3AC1:1;
    unsigned ECCP3AS0:1;
    unsigned ECCP3AS1:1;
    unsigned ECCP3AS2:1;
    unsigned ECCP3ASE:1;
  };
} ECCP3ASbits;
extern volatile near unsigned char       CCP2CON;
extern volatile near union {
  struct {
    unsigned CCP2M:4;
    unsigned DC2B:2;
    unsigned P2M:2;
  };
  struct {
    unsigned CCP2M0:1;
    unsigned CCP2M1:1;
    unsigned CCP2M2:1;
    unsigned CCP2M3:1;
    unsigned DC2B0:1;
    unsigned DC2B1:1;
    unsigned P2M0:1;
    unsigned P2M1:1;
  };
  struct {
    unsigned :4;
    unsigned CCP2Y:1;
    unsigned CCP2X:1;
  };
} CCP2CONbits;
extern volatile near unsigned char       ECCP2CON;
extern volatile near union {
  struct {
    unsigned CCP2M:4;
    unsigned DC2B:2;
    unsigned P2M:2;
  };
  struct {
    unsigned CCP2M0:1;
    unsigned CCP2M1:1;
    unsigned CCP2M2:1;
    unsigned CCP2M3:1;
    unsigned DC2B0:1;
    unsigned DC2B1:1;
    unsigned P2M0:1;
    unsigned P2M1:1;
  };
  struct {
    unsigned :4;
    unsigned CCP2Y:1;
    unsigned CCP2X:1;
  };
} ECCP2CONbits;
extern volatile near unsigned            CCPR2;
extern volatile near unsigned char       CCPR2L;
extern volatile near unsigned char       CCPR2H;
extern volatile near unsigned char       ECCP2DEL;
extern volatile near union {
  struct {
    unsigned PDC:7;
    unsigned PRSEN:1;
  };
  struct {
    unsigned PDC0:1;
    unsigned PDC1:1;
    unsigned PDC2:1;
    unsigned PDC3:1;
    unsigned PDC4:1;
    unsigned PDC5:1;
    unsigned PDC6:1;
  };
  struct {
    unsigned P2DC0:1;
    unsigned P2DC1:1;
    unsigned P2DC2:1;
    unsigned P2DC3:1;
    unsigned P2DC4:1;
    unsigned P2DC5:1;
    unsigned P2DC6:1;
    unsigned P2RSEN:1;
  };
} ECCP2DELbits;
extern volatile near unsigned char       ECCP2AS;
extern volatile near union {
  struct {
    unsigned PSSBD:2;
    unsigned PSSAC:2;
    unsigned ECCPAS:3;
    unsigned ECCPASE:1;
  };
  struct {
    unsigned PSSBD0:1;
    unsigned PSSBD1:1;
    unsigned PSSAC0:1;
    unsigned PSSAC1:1;
    unsigned ECCPAS0:1;
    unsigned ECCPAS1:1;
    unsigned ECCPAS2:1;
  };
  struct {
    unsigned PSS2BD0:1;
    unsigned PSS2BD1:1;
    unsigned PSS2AC0:1;
    unsigned PSS2AC1:1;
    unsigned ECCP2AS0:1;
    unsigned ECCP2AS1:1;
    unsigned ECCP2AS2:1;
    unsigned ECCP2ASE:1;
  };
} ECCP2ASbits;
extern volatile near unsigned char       CCP1CON;
extern volatile near union {
  struct {
    unsigned CCP1M:4;
    unsigned DC1B:2;
    unsigned P1M:2;
  };
  struct {
    unsigned CCP1M0:1;
    unsigned CCP1M1:1;
    unsigned CCP1M2:1;
    unsigned CCP1M3:1;
    unsigned DC1B0:1;
    unsigned DC1B1:1;
    unsigned P1M0:1;
    unsigned P1M1:1;
  };
  struct {
    unsigned :4;
    unsigned CCP1Y:1;
    unsigned CCP1X:1;
  };
} CCP1CONbits;
extern volatile near unsigned char       ECCP1CON;
extern volatile near union {
  struct {
    unsigned CCP1M:4;
    unsigned DC1B:2;
    unsigned P1M:2;
  };
  struct {
    unsigned CCP1M0:1;
    unsigned CCP1M1:1;
    unsigned CCP1M2:1;
    unsigned CCP1M3:1;
    unsigned DC1B0:1;
    unsigned DC1B1:1;
    unsigned P1M0:1;
    unsigned P1M1:1;
  };
  struct {
    unsigned :4;
    unsigned CCP1Y:1;
    unsigned CCP1X:1;
  };
} ECCP1CONbits;
extern volatile near unsigned            CCPR1;
extern volatile near unsigned char       CCPR1L;
extern volatile near unsigned char       CCPR1H;
extern volatile near unsigned char       ECCP1DEL;
extern volatile near union {
  struct {
    unsigned PDC:7;
    unsigned PRSEN:1;
  };
  struct {
    unsigned PDC0:1;
    unsigned PDC1:1;
    unsigned PDC2:1;
    unsigned PDC3:1;
    unsigned PDC4:1;
    unsigned PDC5:1;
    unsigned PDC6:1;
  };
  struct {
    unsigned P1DC0:1;
    unsigned P1DC1:1;
    unsigned P1DC2:1;
    unsigned P1DC3:1;
    unsigned P1DC4:1;
    unsigned P1DC5:1;
    unsigned P1DC6:1;
    unsigned P1RSEN:1;
  };
} ECCP1DELbits;
extern volatile near unsigned char       ECCP1AS;
extern volatile near union {
  struct {
    unsigned PSSBD:2;
    unsigned PSSAC:2;
    unsigned ECCPAS:3;
    unsigned ECCPASE:1;
  };
  struct {
    unsigned PSSBD0:1;
    unsigned PSSBD1:1;
    unsigned PSSAC0:1;
    unsigned PSSAC1:1;
    unsigned ECCPAS0:1;
    unsigned ECCPAS1:1;
    unsigned ECCPAS2:1;
  };
  struct {
    unsigned PSS1BD0:1;
    unsigned PSS1BD1:1;
    unsigned PSS1AC0:1;
    unsigned PSS1AC1:1;
    unsigned ECCP1AS0:1;
    unsigned ECCP1AS1:1;
    unsigned ECCP1AS2:1;
    unsigned ECCP1ASE:1;
  };
} ECCP1ASbits;
extern volatile near unsigned char       WDTCON;
extern volatile near union {
  struct {
    unsigned SWDTEN:1;
    unsigned :3;
    unsigned ADSHR:1;
    unsigned :1;
    unsigned LVDSTAT:1;
    unsigned REGSLP:1;
  };
  struct {
    unsigned SWDTE:1;
    unsigned :3;
    unsigned DEVCFG:1;
  };
} WDTCONbits;
extern volatile near unsigned char       ADCON1;
extern volatile near union {
  struct {
    unsigned ADCS:3;
    unsigned ACQT:3;
    unsigned ADCAL:1;
    unsigned ADFM:1;
  };
  struct {
    unsigned ADCS0:1;
    unsigned ADCS1:1;
    unsigned ADCS2:1;
    unsigned ACQT0:1;
    unsigned ACQT1:1;
    unsigned ACQT2:1;
  };
} ADCON1bits;
extern volatile near unsigned char       ANCON0;
extern volatile near union {
  struct {
    unsigned PCFGL:8;
  };
  struct {
    unsigned PCFG0:1;
    unsigned PCFG1:1;
    unsigned PCFG2:1;
    unsigned PCFG3:1;
    unsigned PCFG4:1;
    unsigned :2;
    unsigned PCFG7:1;
  };
} ANCON0bits;
extern volatile near unsigned char       ADCON0;
extern volatile near union {
  struct {
    unsigned ADON:1;
    unsigned GO_NOT_DONE:1;
    unsigned CHS:4;
    unsigned VCFG:2;
  };
  struct {
    unsigned :1;
    unsigned DONE:1;
    unsigned CHS0:1;
    unsigned CHS1:1;
    unsigned CHS2:1;
    unsigned CHS3:1;
    unsigned VCFG0:1;
    unsigned VCFG1:1;
  };
  struct {
    unsigned :1;
    unsigned GO_DONE:1;
  };
  struct {
    unsigned :1;
    unsigned GO:1;
  };
  struct {
    unsigned :1;
    unsigned NOT_DONE:1;
  };
} ADCON0bits;
extern volatile near unsigned char       ANCON1;
extern volatile near union {
  struct {
    unsigned PCFGH:8;
  };
  struct {
    unsigned :2;
    unsigned PCFG10:1;
    unsigned PCFG11:1;
    unsigned PCFG12:1;
    unsigned PCFG13:1;
    unsigned PCFG14:1;
    unsigned PCFG15:1;
  };
} ANCON1bits;
extern volatile near unsigned            ADRES;
extern volatile near unsigned char       ADRESL;
extern volatile near unsigned char       ADRESH;
extern volatile near unsigned char       SSP1CON2;
extern volatile near union {
  struct {
    unsigned SEN:1;
    unsigned RSEN:1;
    unsigned PEN:1;
    unsigned RCEN:1;
    unsigned ACKEN:1;
    unsigned ACKDT:1;
    unsigned ACKSTAT:1;
    unsigned GCEN:1;
  };
  struct {
    unsigned :1;
    unsigned ADMSK1:1;
    unsigned ADMSK2:1;
    unsigned ADMSK3:1;
    unsigned ADMSK4:1;
    unsigned ADMSK5:1;
  };
} SSP1CON2bits;
extern volatile near unsigned char       SSPCON2;
extern volatile near union {
  struct {
    unsigned SEN:1;
    unsigned RSEN:1;
    unsigned PEN:1;
    unsigned RCEN:1;
    unsigned ACKEN:1;
    unsigned ACKDT:1;
    unsigned ACKSTAT:1;
    unsigned GCEN:1;
  };
  struct {
    unsigned :1;
    unsigned ADMSK1:1;
    unsigned ADMSK2:1;
    unsigned ADMSK3:1;
    unsigned ADMSK4:1;
    unsigned ADMSK5:1;
  };
} SSPCON2bits;
extern volatile near unsigned char       SSP1CON1;
extern volatile near union {
  struct {
    unsigned SSPM:4;
    unsigned CKP:1;
    unsigned SSPEN:1;
    unsigned SSPOV:1;
    unsigned WCOL:1;
  };
  struct {
    unsigned SSPM0:1;
    unsigned SSPM1:1;
    unsigned SSPM2:1;
    unsigned SSPM3:1;
  };
} SSP1CON1bits;
extern volatile near unsigned char       SSPCON1;
extern volatile near union {
  struct {
    unsigned SSPM:4;
    unsigned CKP:1;
    unsigned SSPEN:1;
    unsigned SSPOV:1;
    unsigned WCOL:1;
  };
  struct {
    unsigned SSPM0:1;
    unsigned SSPM1:1;
    unsigned SSPM2:1;
    unsigned SSPM3:1;
  };
} SSPCON1bits;
extern volatile near unsigned char       SSP1STAT;
extern volatile near union {
  struct {
    unsigned BF:1;
    unsigned UA:1;
    unsigned R_NOT_W:1;
    unsigned S:1;
    unsigned P:1;
    unsigned D_NOT_A:1;
    unsigned CKE:1;
    unsigned SMP:1;
  };
  struct {
    unsigned :2;
    unsigned R_W:1;
    unsigned :2;
    unsigned D_A:1;
  };
  struct {
    unsigned :2;
    unsigned I2C_READ:1;
    unsigned I2C_START:1;
    unsigned I2C_STOP:1;
    unsigned I2C_DAT:1;
  };
  struct {
    unsigned :2;
    unsigned NOT_W:1;
    unsigned :2;
    unsigned NOT_A:1;
  };
  struct {
    unsigned :2;
    unsigned NOT_WRITE:1;
    unsigned :2;
    unsigned NOT_ADDRESS:1;
  };
  struct {
    unsigned :2;
    unsigned READ_WRITE:1;
    unsigned :2;
    unsigned DATA_ADDRESS:1;
  };
  struct {
    unsigned :2;
    unsigned R:1;
    unsigned :2;
    unsigned D:1;
  };
} SSP1STATbits;
extern volatile near unsigned char       SSPSTAT;
extern volatile near union {
  struct {
    unsigned BF:1;
    unsigned UA:1;
    unsigned R_NOT_W:1;
    unsigned S:1;
    unsigned P:1;
    unsigned D_NOT_A:1;
    unsigned CKE:1;
    unsigned SMP:1;
  };
  struct {
    unsigned :2;
    unsigned R_W:1;
    unsigned :2;
    unsigned D_A:1;
  };
  struct {
    unsigned :2;
    unsigned I2C_READ:1;
    unsigned I2C_START:1;
    unsigned I2C_STOP:1;
    unsigned I2C_DAT:1;
  };
  struct {
    unsigned :2;
    unsigned NOT_W:1;
    unsigned :2;
    unsigned NOT_A:1;
  };
  struct {
    unsigned :2;
    unsigned NOT_WRITE:1;
    unsigned :2;
    unsigned NOT_ADDRESS:1;
  };
  struct {
    unsigned :2;
    unsigned READ_WRITE:1;
    unsigned :2;
    unsigned DATA_ADDRESS:1;
  };
  struct {
    unsigned :2;
    unsigned R:1;
    unsigned :2;
    unsigned D:1;
  };
} SSPSTATbits;
extern volatile near unsigned char       SSP1ADD;
extern volatile near unsigned char       SSP1MSK;
extern volatile near struct {
  unsigned MSK0:1;
  unsigned MSK1:1;
  unsigned MSK2:1;
  unsigned MSK3:1;
  unsigned MSK4:1;
  unsigned MSK5:1;
  unsigned MSK6:1;
  unsigned MSK7:1;
} SSP1MSKbits;
extern volatile near unsigned char       SSPADD;
extern volatile near unsigned char       SSP1BUF;
extern volatile near unsigned char       SSPBUF;
extern volatile near unsigned char       T2CON;
extern volatile near union {
  struct {
    unsigned T2CKPS:2;
    unsigned TMR2ON:1;
    unsigned TOUTPS:4;
  };
  struct {
    unsigned T2CKPS0:1;
    unsigned T2CKPS1:1;
    unsigned :1;
    unsigned T2OUTPS0:1;
    unsigned T2OUTPS1:1;
    unsigned T2OUTPS2:1;
    unsigned T2OUTPS3:1;
  };
} T2CONbits;
extern volatile near unsigned char       MEMCON;
extern volatile near union {
  struct {
    unsigned WM:2;
    unsigned :2;
    unsigned WAIT:2;
    unsigned :1;
    unsigned EBDIS:1;
  };
  struct {
    unsigned WM0:1;
    unsigned WM1:1;
    unsigned :2;
    unsigned WAIT0:1;
    unsigned WAIT1:1;
    unsigned :1;
    unsigned EDBIS:1;
  };
} MEMCONbits;
extern volatile near unsigned char       PR2;
extern volatile near unsigned char       PADCFG1;
extern volatile near union {
  struct {
    unsigned PMPTL:1;
  };
  struct {
    unsigned PMPTTL:1;
  };
} PADCFG1bits;
extern volatile near unsigned char       TMR2;
extern volatile near unsigned char       ODCON3;
extern volatile near struct {
  unsigned SPI1OD:1;
  unsigned SPI2OD:1;
} ODCON3bits;
extern volatile near unsigned char       T1CON;
extern volatile near union {
  struct {
    unsigned TMR1ON:1;
    unsigned TMR1CS:1;
    unsigned NOT_T1SYNC:1;
    unsigned T1OSCEN:1;
    unsigned T1CKPS:2;
    unsigned T1RUN:1;
    unsigned RD16:1;
  };
  struct {
    unsigned :2;
    unsigned T1SYNC:1;
    unsigned :1;
    unsigned T1CKPS0:1;
    unsigned T1CKPS1:1;
  };
  struct {
    unsigned :2;
    unsigned T1INSYNC:1;
  };
} T1CONbits;
extern volatile near unsigned char       ODCON2;
extern volatile near union {
  struct {
    unsigned USART1OD:1;
    unsigned USART2OD:1;
  };
  struct {
    unsigned U1OD:1;
    unsigned U2OD:1;
  };
} ODCON2bits;
extern volatile near unsigned char       TMR1L;
extern volatile near unsigned char       ODCON1;
extern volatile near struct {
  unsigned ECCP1OD:1;
  unsigned ECCP2OD:1;
  unsigned ECCP3OD:1;
  unsigned CCP4OD:1;
  unsigned CCP5OD:1;
} ODCON1bits;
extern volatile near unsigned char       TMR1H;
extern volatile near unsigned char       RCON;
extern volatile near union {
  struct {
    unsigned NOT_BOR:1;
    unsigned NOT_POR:1;
    unsigned NOT_PD:1;
    unsigned NOT_TO:1;
    unsigned NOT_RI:1;
    unsigned NOT_CM:1;
    unsigned :1;
    unsigned IPEN:1;
  };
  struct {
    unsigned BOR:1;
    unsigned POR:1;
    unsigned PD:1;
    unsigned TO:1;
    unsigned RI:1;
    unsigned CM:1;
  };
} RCONbits;
extern volatile near unsigned char       CM2CON;
extern volatile near union {
  struct {
    unsigned CCH:2;
    unsigned CREF:1;
    unsigned EVPOL:2;
    unsigned CPOL:1;
    unsigned COE:1;
    unsigned CON:1;
  };
  struct {
    unsigned C1CH0:1;
    unsigned C1CH1:1;
    unsigned :1;
    unsigned EVPOL0:1;
    unsigned EVPOL1:1;
  };
  struct {
    unsigned CCH0:1;
    unsigned CCH1:1;
  };
  struct {
    unsigned C1CH02:1;
    unsigned C1CH12:1;
    unsigned CREF2:1;
    unsigned EVPOL02:1;
    unsigned EVPOL12:1;
    unsigned CPOL2:1;
    unsigned COE2:1;
    unsigned CON2:1;
  };
} CM2CONbits;
extern volatile near unsigned char       CM2CON1;
extern volatile near union {
  struct {
    unsigned CCH:2;
    unsigned CREF:1;
    unsigned EVPOL:2;
    unsigned CPOL:1;
    unsigned COE:1;
    unsigned CON:1;
  };
  struct {
    unsigned C1CH0:1;
    unsigned C1CH1:1;
    unsigned :1;
    unsigned EVPOL0:1;
    unsigned EVPOL1:1;
  };
  struct {
    unsigned CCH0:1;
    unsigned CCH1:1;
  };
  struct {
    unsigned C1CH02:1;
    unsigned C1CH12:1;
    unsigned CREF2:1;
    unsigned EVPOL02:1;
    unsigned EVPOL12:1;
    unsigned CPOL2:1;
    unsigned COE2:1;
    unsigned CON2:1;
  };
} CM2CON1bits;
extern volatile near unsigned char       CM1CON;
extern volatile near union {
  struct {
    unsigned CCH:2;
    unsigned CREF:1;
    unsigned EVPOL:2;
    unsigned CPOL:1;
    unsigned COE:1;
    unsigned CON:1;
  };
  struct {
    unsigned C1CH0:1;
    unsigned C1CH1:1;
    unsigned :1;
    unsigned EVPOL0:1;
    unsigned EVPOL1:1;
  };
  struct {
    unsigned CCH0:1;
    unsigned CCH1:1;
  };
} CM1CONbits;
extern volatile near unsigned char       CM1CON1;
extern volatile near union {
  struct {
    unsigned CCH:2;
    unsigned CREF:1;
    unsigned EVPOL:2;
    unsigned CPOL:1;
    unsigned COE:1;
    unsigned CON:1;
  };
  struct {
    unsigned C1CH0:1;
    unsigned C1CH1:1;
    unsigned :1;
    unsigned EVPOL0:1;
    unsigned EVPOL1:1;
  };
  struct {
    unsigned CCH0:1;
    unsigned CCH1:1;
  };
} CM1CON1bits;
extern volatile near unsigned char       OSCCON;
extern volatile near union {
  struct {
    unsigned SCS:2;
    unsigned :1;
    unsigned OSTS:1;
    unsigned IRCF:3;
    unsigned IDLEN:1;
  };
  struct {
    unsigned SCS0:1;
    unsigned SCS1:1;
    unsigned :2;
    unsigned IRCF0:1;
    unsigned IRCF1:1;
    unsigned IRCF2:1;
  };
} OSCCONbits;
extern volatile near unsigned char       REFOCON;
extern volatile near union {
  struct {
    unsigned RODIV:4;
    unsigned ROSEL:1;
    unsigned ROSSLP:1;
    unsigned :1;
    unsigned ROON:1;
  };
  struct {
    unsigned RODIV0:1;
    unsigned RODIV1:1;
    unsigned RODIV2:1;
    unsigned RODIV3:1;
  };
} REFOCONbits;
extern volatile near unsigned char       T0CON;
extern volatile near union {
  struct {
    unsigned T0PS:3;
    unsigned PSA:1;
    unsigned T0SE:1;
    unsigned T0CS:1;
    unsigned T08BIT:1;
    unsigned TMR0ON:1;
  };
  struct {
    unsigned T0PS0:1;
    unsigned T0PS1:1;
    unsigned T0PS2:1;
    unsigned T0PS3:1;
  };
} T0CONbits;
extern volatile near unsigned char       TMR0L;
extern volatile near unsigned char       TMR0H;
extern          near unsigned char       STATUS;
extern          near struct {
  unsigned C:1;
  unsigned DC:1;
  unsigned Z:1;
  unsigned OV:1;
  unsigned N:1;
} STATUSbits;
extern          near unsigned            FSR2;
extern          near unsigned char       FSR2L;
extern          near unsigned char       FSR2H;
extern volatile near unsigned char       PLUSW2;
extern volatile near unsigned char       PREINC2;
extern volatile near unsigned char       POSTDEC2;
extern volatile near unsigned char       POSTINC2;
extern          near unsigned char       INDF2;
extern          near unsigned char       BSR;
extern          near unsigned            FSR1;
extern          near unsigned char       FSR1L;
extern          near unsigned char       FSR1H;
extern volatile near unsigned char       PLUSW1;
extern volatile near unsigned char       PREINC1;
extern volatile near unsigned char       POSTDEC1;
extern volatile near unsigned char       POSTINC1;
extern          near unsigned char       INDF1;
extern          near unsigned char       WREG;
extern          near unsigned            FSR0;
extern          near unsigned char       FSR0L;
extern          near unsigned char       FSR0H;
extern volatile near unsigned char       PLUSW0;
extern volatile near unsigned char       PREINC0;
extern volatile near unsigned char       POSTDEC0;
extern volatile near unsigned char       POSTINC0;
extern          near unsigned char       INDF0;
extern volatile near unsigned char       INTCON3;
extern volatile near union {
  struct {
    unsigned INT1IF:1;
    unsigned INT2IF:1;
    unsigned INT3IF:1;
    unsigned INT1IE:1;
    unsigned INT2IE:1;
    unsigned INT3IE:1;
    unsigned INT1IP:1;
    unsigned INT2IP:1;
  };
  struct {
    unsigned INT1F:1;
    unsigned INT2F:1;
    unsigned INT3F:1;
    unsigned INT1E:1;
    unsigned INT2E:1;
    unsigned INT3E:1;
    unsigned INT1P:1;
    unsigned INT2P:1;
  };
} INTCON3bits;
extern volatile near unsigned char       INTCON2;
extern volatile near union {
  struct {
    unsigned RBIP:1;
    unsigned INT3IP:1;
    unsigned TMR0IP:1;
    unsigned INTEDG3:1;
    unsigned INTEDG2:1;
    unsigned INTEDG1:1;
    unsigned INTEDG0:1;
    unsigned NOT_RBPU:1;
  };
  struct {
    unsigned :1;
    unsigned INT3P:1;
    unsigned T0IP:1;
    unsigned :4;
    unsigned RBPU:1;
  };
} INTCON2bits;
extern volatile near unsigned char       INTCON;
extern volatile near union {
  struct {
    unsigned RBIF:1;
    unsigned INT0IF:1;
    unsigned TMR0IF:1;
    unsigned RBIE:1;
    unsigned INT0IE:1;
    unsigned TMR0IE:1;
    unsigned PEIE_GIEL:1;
    unsigned GIE_GIEH:1;
  };
  struct {
    unsigned :1;
    unsigned INT0F:1;
    unsigned T0IF:1;
    unsigned :1;
    unsigned INT0E:1;
    unsigned T0IE:1;
    unsigned PEIE:1;
    unsigned GIE:1;
  };
  struct {
    unsigned :6;
    unsigned GIEL:1;
    unsigned GIEH:1;
  };
} INTCONbits;
extern          near unsigned            PROD;
extern          near unsigned char       PRODL;
extern          near unsigned char       PRODH;
extern volatile near unsigned char       TABLAT;
extern volatile near unsigned short long TBLPTR;
extern volatile near unsigned char       TBLPTRL;
extern volatile near unsigned char       TBLPTRH;
extern volatile near unsigned char       TBLPTRU;
extern volatile near unsigned short long PC;
extern volatile near unsigned char       PCL;
extern volatile near unsigned char       PCLATH;
extern volatile near unsigned char       PCLATU;
extern volatile near unsigned char       STKPTR;
extern volatile near union {
  struct {
    unsigned STKPTR:5;
    unsigned :1;
    unsigned STKUNF:1;
    unsigned STKFUL:1;
  };
  struct {
    unsigned STKPTR0:1;
    unsigned STKPTR1:1;
    unsigned STKPTR2:1;
    unsigned STKPTR3:1;
    unsigned STKPTR4:1;
    unsigned :2;
    unsigned STKOVF:1;
  };
  struct {
    unsigned SP0:1;
    unsigned SP1:1;
    unsigned SP2:1;
    unsigned SP3:1;
    unsigned SP4:1;
  };
} STKPTRbits;
extern          near unsigned short long TOS;
extern          near unsigned char       TOSL;
extern          near unsigned char       TOSH;
extern          near unsigned char       TOSU;

#pragma varlocate 15 PMSTAT
#pragma varlocate 15 PMSTATL
#pragma varlocate 15 PMSTATLbits
#pragma varlocate 15 PMSTATH
#pragma varlocate 15 PMSTATHbits
#pragma varlocate 15 PMEL
#pragma varlocate 15 PMELbits
#pragma varlocate 15 PMEN
#pragma varlocate 15 PMEH
#pragma varlocate 15 PMEHbits
#pragma varlocate 15 PMDIN2
#pragma varlocate 15 PMDIN2L
#pragma varlocate 15 PMDIN2H
#pragma varlocate 15 PMDOUT2
#pragma varlocate 15 PMDOUT2L
#pragma varlocate 15 PMDOUT2H
#pragma varlocate 15 PMMODE
#pragma varlocate 15 PMMODEL
#pragma varlocate 15 PMMODELbits
#pragma varlocate 15 PMMODEH
#pragma varlocate 15 PMMODEHbits
#pragma varlocate 15 PMCON
#pragma varlocate 15 PMCONL
#pragma varlocate 15 PMCONLbits
#pragma varlocate 15 PMCONH
#pragma varlocate 15 PMCONHbits
#pragma varlocate 15 UEP0
#pragma varlocate 15 UEP0bits
#pragma varlocate 15 UEP1
#pragma varlocate 15 UEP1bits
#pragma varlocate 15 UEP2
#pragma varlocate 15 UEP2bits
#pragma varlocate 15 UEP3
#pragma varlocate 15 UEP3bits
#pragma varlocate 15 UEP4
#pragma varlocate 15 UEP4bits
#pragma varlocate 15 UEP5
#pragma varlocate 15 UEP5bits
#pragma varlocate 15 UEP6
#pragma varlocate 15 UEP6bits
#pragma varlocate 15 UEP7
#pragma varlocate 15 UEP7bits
#pragma varlocate 15 UEP8
#pragma varlocate 15 UEP8bits
#pragma varlocate 15 UEP9
#pragma varlocate 15 UEP9bits
#pragma varlocate 15 UEP10
#pragma varlocate 15 UEP10bits
#pragma varlocate 15 UEP11
#pragma varlocate 15 UEP11bits
#pragma varlocate 15 UEP12
#pragma varlocate 15 UEP12bits
#pragma varlocate 15 UEP13
#pragma varlocate 15 UEP13bits
#pragma varlocate 15 UEP14
#pragma varlocate 15 UEP14bits
#pragma varlocate 15 UEP15
#pragma varlocate 15 UEP15bits
#pragma varlocate 15 UIE
#pragma varlocate 15 UIEbits
#pragma varlocate 15 UEIE
#pragma varlocate 15 UEIEbits
#pragma varlocate 15 UADDR
#pragma varlocate 15 UADDRbits
#pragma varlocate 15 UCFG
#pragma varlocate 15 UCFGbits


#line 2999 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
 
#line 3001 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3002 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"


#line 3005 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
 
#line 3007 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3008 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3009 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3010 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"

#line 3012 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3013 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3014 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3015 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 3016 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"


#line 3020 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
 
#line 3022 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"


#line 3025 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18f85j50.h"
#line 411 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"

#line 413 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 415 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 417 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 419 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 421 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 423 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 425 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 427 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 429 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 431 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 433 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 435 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 437 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 439 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 441 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 443 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 445 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 447 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 449 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 451 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 453 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 455 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 457 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 459 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 461 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 463 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 465 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 467 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 469 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 471 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 473 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 475 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 477 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 479 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 481 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 483 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 485 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 487 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 489 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 491 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 493 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 495 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 497 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 499 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 501 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 503 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 505 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 507 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 509 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 511 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 513 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 515 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 517 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 519 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 521 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 523 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 525 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 527 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 529 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 531 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 533 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 535 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 537 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 539 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 541 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 543 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 545 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 547 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 549 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 551 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 553 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 555 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 557 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 559 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 561 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 563 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 565 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 567 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 569 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 571 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 573 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 575 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 577 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 579 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 581 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 583 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 585 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 587 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 589 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 591 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 593 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 595 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 597 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"

#line 599 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/p18cxxx.h"
#line 56 "./Compiler.h"



#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"

#line 3 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"

#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdarg.h"
 


#line 5 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdarg.h"

typedef void* va_list;
#line 8 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdarg.h"
#line 9 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdarg.h"
#line 10 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdarg.h"
#line 11 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdarg.h"
#line 12 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdarg.h"
#line 4 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"

#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
 

#line 10 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"

#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"

#line 34 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"

#line 41 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
#line 45 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stddef.h"
#line 5 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"



#line 9 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"
 
#line 11 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"

#line 13 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"


typedef unsigned char FILE;

 
#line 19 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"
#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"

extern FILE *stderr;
extern FILE *stdout;


int putc (auto char c, auto FILE *f);
int vsprintf (auto char *buf, auto const far  rom char *fmt, auto va_list ap);
int vprintf (auto const far  rom char *fmt, auto va_list ap);
int sprintf (auto char *buf, auto const far  rom char *fmt, ...);
int printf (auto const far  rom char *fmt, ...);
int fprintf (auto FILE *f, auto const far  rom char *fmt, ...);
int vfprintf (auto FILE *f, auto const far  rom char *fmt, auto va_list ap);
int puts (auto const far  rom char *s);
int fputs (auto const far  rom char *s, auto FILE *f);

#line 36 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdio.h"
#line 59 "./Compiler.h"

#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
 
#line 6 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 9 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 16 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 28 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 47 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 58 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 71 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 83 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 95 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 107 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 112 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 116 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 124 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"

#line 136 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
#line 140 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
#line 149 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
#line 151 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/stdlib.h"
#line 60 "./Compiler.h"

#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 7 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 22 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 25 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 55 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 67 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 83 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 97 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 113 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 128 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 141 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 147 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 161 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 167 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 183 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 199 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 210 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 222 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 238 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 249 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 262 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 305 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 321 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 339 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 349 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 358 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 367 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 379 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 389 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 398 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 407 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 417 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 426 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 434 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 443 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 451 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 460 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 470 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 479 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 487 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 496 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 504 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 513 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 523 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 532 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 541 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 551 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 560 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 568 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 577 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 585 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 594 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 604 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 613 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 622 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 631 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 639 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 647 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 655 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 663 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 671 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 679 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 688 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 696 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 704 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 712 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 720 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 729 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 737 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 745 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 754 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 762 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 771 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 778 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 785 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 792 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 798 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"

#line 805 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 814 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 816 "C:/Program Files (x86)/Microchip/mplabc18/v3.46/h/string.h"
#line 61 "./Compiler.h"


#line 64 "./Compiler.h"
#line 65 "./Compiler.h"




#line 70 "./Compiler.h"
#line 71 "./Compiler.h"

#line 73 "./Compiler.h"

	
#line 76 "./Compiler.h"
#line 77 "./Compiler.h"
#line 78 "./Compiler.h"
#line 79 "./Compiler.h"
	
	
#line 82 "./Compiler.h"
#line 88 "./Compiler.h"
    


#line 92 "./Compiler.h"
#line 96 "./Compiler.h"
#line 99 "./Compiler.h"
#line 102 "./Compiler.h"
#line 109 "./Compiler.h"
#line 110 "./Compiler.h"



#line 114 "./Compiler.h"
#line 99 "./usb_device.h"

#line 1 "./usb_ch9.h"

#line 45 "./usb_ch9.h"
 


#line 78 "./usb_ch9.h"
 




#line 109 "./usb_ch9.h"

#line 135 "./usb_ch9.h"

#line 160 "./usb_ch9.h"

#line 181 "./usb_ch9.h"

#line 235 "./usb_ch9.h"
#line 425 "./usb_ch9.h"

#line 427 "./usb_ch9.h"
 

#line 100 "./usb_device.h"

#line 1 "./usb_hal.h"

#line 41 "./usb_hal.h"
 


#line 84 "./usb_hal.h"
 

#line 93 "./usb_hal.h"

#line 96 "./usb_hal.h"

#line 127 "./usb_hal.h"

#line 130 "./usb_hal.h"

#line 164 "./usb_hal.h"

#line 168 "./usb_hal.h"
#line 170 "./usb_hal.h"
#line 172 "./usb_hal.h"
#line 175 "./usb_hal.h"

#line 183 "./usb_hal.h"

#line 191 "./usb_hal.h"

#line 215 "./usb_hal.h"

#line 241 "./usb_hal.h"

#line 253 "./usb_hal.h"

#line 282 "./usb_hal.h"

#line 288 "./usb_hal.h"

#line 328 "./usb_hal.h"

#line 363 "./usb_hal.h"

#line 367 "./usb_hal.h"

#line 400 "./usb_hal.h"

#line 404 "./usb_hal.h"

#line 433 "./usb_hal.h"

#line 438 "./usb_hal.h"

#line 477 "./usb_hal.h"

#line 537 "./usb_hal.h"

#line 577 "./usb_hal.h"
#line 583 "./usb_hal.h"
#line 590 "./usb_hal.h"

#line 597 "./usb_hal.h"
#line 601 "./usb_hal.h"

#line 627 "./usb_hal.h"
#line 632 "./usb_hal.h"
#line 634 "./usb_hal.h"
#line 635 "./usb_hal.h"

#line 637 "./usb_hal.h"
 

#line 101 "./usb_device.h"

#line 1 "./usb_config.h"

#line 43 "./usb_config.h"
 


#line 47 "./usb_config.h"
 

#line 130 "./usb_config.h"
#line 102 "./usb_device.h"
 
                        
                        
                        
 


#line 115 "./usb_device.h"
 
#line 117 "./usb_device.h"
#line 118 "./usb_device.h"
#line 119 "./usb_device.h"
#line 120 "./usb_device.h"
#line 121 "./usb_device.h"
#line 122 "./usb_device.h"
#line 123 "./usb_device.h"
#line 124 "./usb_device.h"
#line 125 "./usb_device.h"
#line 126 "./usb_device.h"
#line 127 "./usb_device.h"
#line 128 "./usb_device.h"
#line 129 "./usb_device.h"
#line 130 "./usb_device.h"
#line 131 "./usb_device.h"
#line 132 "./usb_device.h"
#line 133 "./usb_device.h"
#line 134 "./usb_device.h"
#line 135 "./usb_device.h"
#line 136 "./usb_device.h"
#line 137 "./usb_device.h"
#line 138 "./usb_device.h"
#line 139 "./usb_device.h"
#line 140 "./usb_device.h"
#line 141 "./usb_device.h"
#line 142 "./usb_device.h"
#line 143 "./usb_device.h"
#line 144 "./usb_device.h"
#line 145 "./usb_device.h"
#line 146 "./usb_device.h"
#line 147 "./usb_device.h"
#line 148 "./usb_device.h"

 
#line 151 "./usb_device.h"
#line 152 "./usb_device.h"
#line 153 "./usb_device.h"
#line 154 "./usb_device.h"
#line 155 "./usb_device.h"

 
#line 158 "./usb_device.h"
#line 159 "./usb_device.h"
#line 160 "./usb_device.h"

#line 162 "./usb_device.h"
#line 163 "./usb_device.h"
#line 164 "./usb_device.h"
#line 165 "./usb_device.h"

 
#line 168 "./usb_device.h"
#line 169 "./usb_device.h"
#line 170 "./usb_device.h"
#line 171 "./usb_device.h"

 
#line 174 "./usb_device.h"
#line 175 "./usb_device.h"
#line 176 "./usb_device.h"

#line 178 "./usb_device.h"
#line 179 "./usb_device.h"



#line 183 "./usb_device.h"
#line 184 "./usb_device.h"


#line 189 "./usb_device.h"
 
typedef union   _CTRL_TRF_SETUP
{
     
    struct  
    {
        BYTE bmRequestType; 
        BYTE bRequest; 
        WORD wValue; 
        WORD wIndex; 
        WORD wLength; 
    };
    struct  
    {
        unsigned :8;
        unsigned :8;
        WORD_VAL W_Value; 
        WORD_VAL W_Index; 
        WORD_VAL W_Length; 
    };
    struct  
    {
        unsigned Recipient:5;   
        unsigned RequestType:2; 
        unsigned DataDir:1;     
        unsigned :8;
        BYTE bFeature;          
        unsigned :8;
        unsigned :8;
        unsigned :8;
        unsigned :8;
        unsigned :8;
    };
    struct  
    {
        unsigned :8;
        unsigned :8;
        BYTE bDscIndex;         
        BYTE bDescriptorType;          
        WORD wLangID;           
        unsigned :8;
        unsigned :8;
    };
    struct  
    {
        unsigned :8;
        unsigned :8;
        BYTE_VAL bDevADR;       
        BYTE bDevADRH;          
        unsigned :8;
        unsigned :8;
        unsigned :8;
        unsigned :8;
    };
    struct  
    {
        unsigned :8;
        unsigned :8;
        BYTE bConfigurationValue;         
        BYTE bCfgRSD;           
        unsigned :8;
        unsigned :8;
        unsigned :8;
        unsigned :8;
    };
    struct  
    {
        unsigned :8;
        unsigned :8;
        BYTE bAltID;            
        BYTE bAltID_H;          
        BYTE bIntfID;           
        BYTE bIntfID_H;         
        unsigned :8;
        unsigned :8;
    };
    struct  
    {
        unsigned :8;
        unsigned :8;
        unsigned :8;
        unsigned :8;
        BYTE bEPID;             
        BYTE bEPID_H;           
        unsigned :8;
        unsigned :8;
    };
    struct  
    {
        unsigned :8;
        unsigned :8;
        unsigned :8;
        unsigned :8;
        unsigned EPNum:4;       
        unsigned :3;
        unsigned EPDir:1;       
        unsigned :8;
        unsigned :8;
        unsigned :8;
    };

     

} CTRL_TRF_SETUP;




typedef struct  
{
    union  
    {
        
        
        BYTE *bRam;
        rom  BYTE *bRom;
        WORD *wRam;
        rom  WORD *wRom;
    }pSrc;
    union  
    {
        struct  
        {
            
            BYTE ctrl_trf_mem          :1;
            BYTE reserved              :5;
            
            
            BYTE includeZero           :1;
            
            BYTE busy                  :1;
        }bits;
        BYTE Val;
    }info;
    WORD_VAL wCount;
}IN_PIPE;

#line 327 "./usb_device.h"
#line 328 "./usb_device.h"
typedef struct  
{
    union  
    {
        
        
        BYTE *bRam;
        WORD *wRam;
    }pDst;
    union  
    {
        struct  
        {
            BYTE reserved              :7;
            
            BYTE busy                  :1;
        }bits;
        BYTE Val;
    }info;
    WORD_VAL wCount;
    void  (*pFunc)(void );
}OUT_PIPE;


#line 353 "./usb_device.h"
#line 354 "./usb_device.h"
#line 355 "./usb_device.h"
#line 356 "./usb_device.h"
#line 357 "./usb_device.h"
#line 358 "./usb_device.h"

#line 360 "./usb_device.h"
#line 361 "./usb_device.h"
#line 362 "./usb_device.h"
#line 363 "./usb_device.h"
#line 364 "./usb_device.h"
#line 365 "./usb_device.h"


#line 369 "./usb_device.h"
 
#line 371 "./usb_device.h"
#line 372 "./usb_device.h"
#line 373 "./usb_device.h"
#line 374 "./usb_device.h"
#line 375 "./usb_device.h"
#line 376 "./usb_device.h"
#line 377 "./usb_device.h"
#line 378 "./usb_device.h"
#line 379 "./usb_device.h"
#line 380 "./usb_device.h"
#line 381 "./usb_device.h"

 
#line 384 "./usb_device.h"
#line 385 "./usb_device.h"

 


#line 391 "./usb_device.h"
 
#line 393 "./usb_device.h"

#line 395 "./usb_device.h"
 
#line 397 "./usb_device.h"

#line 399 "./usb_device.h"
 
#line 401 "./usb_device.h"

#line 403 "./usb_device.h"
 
#line 405 "./usb_device.h"

#line 410 "./usb_device.h"
 
#line 412 "./usb_device.h"

#line 413 "./usb_device.h"
 
#line 415 "./usb_device.h"

#line 419 "./usb_device.h"
 
#line 421 "./usb_device.h"

 
#line 424 "./usb_device.h"
#line 425 "./usb_device.h"
#line 428 "./usb_device.h"

#line 430 "./usb_device.h"
#line 431 "./usb_device.h"
#line 432 "./usb_device.h"

#line 434 "./usb_device.h"
#line 435 "./usb_device.h"
#line 438 "./usb_device.h"

#line 441 "./usb_device.h"
#line 443 "./usb_device.h"
#line 445 "./usb_device.h"
#line 447 "./usb_device.h"
#line 448 "./usb_device.h"
#line 450 "./usb_device.h"


 
#line 454 "./usb_device.h"
#line 455 "./usb_device.h"
#line 456 "./usb_device.h"
#line 457 "./usb_device.h"
#line 458 "./usb_device.h"
                                    

#line 461 "./usb_device.h"
#line 462 "./usb_device.h"

#line 464 "./usb_device.h"
#line 465 "./usb_device.h"

#line 467 "./usb_device.h"
#line 468 "./usb_device.h"

#line 470 "./usb_device.h"
#line 471 "./usb_device.h"

#line 473 "./usb_device.h"
#line 474 "./usb_device.h"
#line 495 "./usb_device.h"





#line 501 "./usb_device.h"

#line 503 "./usb_device.h"
 
    
#line 506 "./usb_device.h"
#line 508 "./usb_device.h"
#line 510 "./usb_device.h"
        extern volatile BDT_ENTRY BDT[(1  + 1) * 4];
#line 512 "./usb_device.h"
#line 514 "./usb_device.h"
#line 516 "./usb_device.h"

    
#line 519 "./usb_device.h"
#line 522 "./usb_device.h"
        extern rom  USB_DEVICE_DESCRIPTOR device_dsc ;
#line 524 "./usb_device.h"

    
    extern rom  BYTE configDescriptor1[];

#line 529 "./usb_device.h"
#line 532 "./usb_device.h"
        extern rom  BYTE *rom  USB_CD_Ptr[] ;
#line 534 "./usb_device.h"

    
    extern rom  BYTE *rom  USB_SD_Ptr[];

#line 539 "./usb_device.h"
    
#line 541 "./usb_device.h"
        extern rom  struct{BYTE report[29 ];} hid_rpt01;
#line 543 "./usb_device.h"
#line 544 "./usb_device.h"

    
    extern volatile CTRL_TRF_SETUP SetupPkt;           
    
    extern volatile BYTE CtrlTrfData[8 ];

#line 551 "./usb_device.h"
    
    extern volatile unsigned char hid_report_out[3 ];
    extern volatile unsigned char hid_report_in[3 ];
#line 555 "./usb_device.h"


#line 558 "./usb_device.h"

 
#line 561 "./usb_device.h"
#line 562 "./usb_device.h"
#line 563 "./usb_device.h"

 
#line 566 "./usb_device.h"
#line 567 "./usb_device.h"
#line 568 "./usb_device.h"

 
#line 571 "./usb_device.h"
#line 572 "./usb_device.h"
#line 573 "./usb_device.h"

 
#line 576 "./usb_device.h"
#line 577 "./usb_device.h"

#line 579 "./usb_device.h"
#line 580 "./usb_device.h"
#line 581 "./usb_device.h"

#line 583 "./usb_device.h"
#line 584 "./usb_device.h"
#line 585 "./usb_device.h"
#line 586 "./usb_device.h"

 
#line 589 "./usb_device.h"
    extern volatile  BYTE USBDeviceState;
    extern volatile  BYTE USBActiveConfiguration;
    extern volatile  IN_PIPE inPipes[1];
    extern volatile  OUT_PIPE outPipes[1];
    extern volatile BDT_ENTRY *pBDTEntryIn[1 +1];
#line 595 "./usb_device.h"

 


#line 653 "./usb_device.h"
 
void USBDeviceTasks(void);


#line 678 "./usb_device.h"
 
void USBDeviceInit(void);


#line 743 "./usb_device.h"
 
#line 745 "./usb_device.h"


#line 809 "./usb_device.h"
 
#line 811 "./usb_device.h"


#line 856 "./usb_device.h"
 
#line 858 "./usb_device.h"


void USBSoftDetach(void);
void USBCtrlEPService(void);
void USBCtrlTrfSetupHandler(void);
void USBCtrlTrfInHandler(void);
void USBCheckStdRequest(void);
void USBStdGetDscHandler(void);
void USBCtrlEPServiceComplete(void);
void USBCtrlTrfTxService(void);
void USBPrepareForNextSetupTrf(void);
void USBCtrlTrfRxService(void);
void USBStdSetCfgHandler(void);
void USBStdGetStatusHandler(void);
void USBStdFeatureReqHandler(void);
void USBCtrlTrfOutHandler(void);
BOOL USBIsTxBusy(BYTE EPNumber);
void USBPut(BYTE EPNum, BYTE Data);
void USBEPService(void);
void USBConfigureEndpoint(BYTE EPNum, BYTE direction);

void USBProtocolResetHandler(void);
void USBWakeFromSuspend(void);
void USBSuspend(void);
void USBStallHandler(void);
volatile volatile BDT_ENTRY*  USBTransferOnePacket(BYTE ep, BYTE dir, BYTE* data, BYTE len);
void USBEnableEndpoint(BYTE ep,BYTE options);

#line 887 "./usb_device.h"
#line 889 "./usb_device.h"
#line 890 "./usb_device.h"
#line 891 "./usb_device.h"

#line 893 "./usb_device.h"
#line 896 "./usb_device.h"
#line 897 "./usb_device.h"
#line 898 "./usb_device.h"

 


#line 945 "./usb_device.h"
 
void USBCBSuspend(void);


#line 981 "./usb_device.h"
 
void USBCBWakeFromSuspend(void);


#line 1012 "./usb_device.h"
 
void USBCB_SOF_Handler(void);


#line 1057 "./usb_device.h"
 
void USBCBErrorHandler(void);


#line 1092 "./usb_device.h"
 
void USBCBCheckOtherReq(void);


#line 1119 "./usb_device.h"
 
void USBCBStdSetDscHandler(void);


#line 1147 "./usb_device.h"
 
void USBCBInitEP(void);


#line 1228 "./usb_device.h"
 
void USBCBSendResume(void);


#line 1262 "./usb_device.h"
 
void USBCBEP0DataReceived(void);




 

#line 1271 "./usb_device.h"
#line 1272 "./usb_device.h"
#line 1273 "./usb_device.h"


#line 1305 "./usb_device.h"
 
#line 1307 "./usb_device.h"


#line 1336 "./usb_device.h"
 
#line 1338 "./usb_device.h"


#line 1365 "./usb_device.h"
 
#line 1367 "./usb_device.h"


#line 1388 "./usb_device.h"
 
#line 1390 "./usb_device.h"


#line 1411 "./usb_device.h"
 
#line 1413 "./usb_device.h"


#line 1441 "./usb_device.h"
 
#line 1443 "./usb_device.h"


#line 1464 "./usb_device.h"
 
#line 1466 "./usb_device.h"


#line 1496 "./usb_device.h"
 
#line 1498 "./usb_device.h"


#line 1528 "./usb_device.h"
 
#line 1530 "./usb_device.h"


#line 1553 "./usb_device.h"
 
#line 1555 "./usb_device.h"


#line 1577 "./usb_device.h"
 
#line 1579 "./usb_device.h"


#line 1600 "./usb_device.h"
 
void USBClearInterruptFlag(BYTE* reg, BYTE flag);


#line 1622 "./usb_device.h"
 
#line 1624 "./usb_device.h"
#line 1625 "./usb_device.h"
#line 1626 "./usb_device.h"
#line 1628 "./usb_device.h"


#line 1649 "./usb_device.h"
 
void USBStallEndpoint(BYTE ep, BYTE dir);

#line 1653 "./usb_device.h"
#line 1654 "./usb_device.h"
#line 1655 "./usb_device.h"
#line 1656 "./usb_device.h"
#line 1657 "./usb_device.h"
#line 1658 "./usb_device.h"
#line 1659 "./usb_device.h"
#line 1660 "./usb_device.h"
#line 1661 "./usb_device.h"
#line 1662 "./usb_device.h"
#line 1663 "./usb_device.h"

#line 1665 "./usb_device.h"
#line 1666 "./usb_device.h"
#line 1667 "./usb_device.h"
#line 1669 "./usb_device.h"

#line 1671 "./usb_device.h"
#line 1672 "./usb_device.h"
#line 1673 "./usb_device.h"
#line 1675 "./usb_device.h"

#line 1677 "./usb_device.h"
#line 1678 "./usb_device.h"
#line 1679 "./usb_device.h"
#line 1681 "./usb_device.h"

#line 1683 "./usb_device.h"
#line 1684 "./usb_device.h"
#line 1685 "./usb_device.h"
#line 1687 "./usb_device.h"

#line 1689 "./usb_device.h"
#line 1690 "./usb_device.h"
#line 1691 "./usb_device.h"
#line 1693 "./usb_device.h"

#line 1695 "./usb_device.h"
#line 1696 "./usb_device.h"
#line 1697 "./usb_device.h"
#line 1699 "./usb_device.h"

#line 1701 "./usb_device.h"
#line 1702 "./usb_device.h"
#line 1703 "./usb_device.h"
#line 1705 "./usb_device.h"

#line 1707 "./usb_device.h"
#line 1708 "./usb_device.h"
#line 1709 "./usb_device.h"
#line 1711 "./usb_device.h"

#line 1713 "./usb_device.h"
#line 1714 "./usb_device.h"
#line 1715 "./usb_device.h"
#line 1717 "./usb_device.h"


#line 1720 "./usb_device.h"
#line 1730 "./usb_device.h"
#line 1731 "./usb_device.h"
#line 1732 "./usb_device.h"
#line 1733 "./usb_device.h"
#line 1734 "./usb_device.h"
#line 1735 "./usb_device.h"
#line 1736 "./usb_device.h"
#line 1737 "./usb_device.h"
#line 1738 "./usb_device.h"
#line 1739 "./usb_device.h"
#line 1740 "./usb_device.h"

#line 1742 "./usb_device.h"
void USBDeviceDetach(void);
void USBDeviceAttach(void);
#line 1745 "./usb_device.h"

#line 1747 "./usb_device.h"
#line 1820 "./usb_device.h"
#line 1892 "./usb_device.h"
#line 1893 "./usb_device.h"
#line 1894 "./usb_device.h"
#line 1895 "./usb_device.h"
#line 1896 "./usb_device.h"
#line 1897 "./usb_device.h"
#line 1901 "./usb_device.h"
#line 1903 "./usb_device.h"
#line 1904 "./usb_device.h"
#line 1905 "./usb_device.h"
#line 1906 "./usb_device.h"
#line 1907 "./usb_device.h"
#line 1908 "./usb_device.h"
#line 1909 "./usb_device.h"
#line 1910 "./usb_device.h"
#line 1911 "./usb_device.h"
#line 1912 "./usb_device.h"
#line 1913 "./usb_device.h"
#line 1914 "./usb_device.h"
#line 1915 "./usb_device.h"
#line 1916 "./usb_device.h"
#line 1917 "./usb_device.h"
#line 1918 "./usb_device.h"
#line 1919 "./usb_device.h"
#line 1920 "./usb_device.h"
#line 1921 "./usb_device.h"
#line 1922 "./usb_device.h"
#line 1923 "./usb_device.h"
#line 1924 "./usb_device.h"
#line 1925 "./usb_device.h"
#line 1926 "./usb_device.h"
#line 1927 "./usb_device.h"
#line 1928 "./usb_device.h"
#line 1929 "./usb_device.h"
#line 1930 "./usb_device.h"
#line 1931 "./usb_device.h"
#line 1932 "./usb_device.h"
#line 1933 "./usb_device.h"
#line 1934 "./usb_device.h"
#line 1935 "./usb_device.h"
#line 1936 "./usb_device.h"
#line 1937 "./usb_device.h"
#line 1938 "./usb_device.h"
#line 1939 "./usb_device.h"
#line 1940 "./usb_device.h"
#line 1941 "./usb_device.h"
#line 1942 "./usb_device.h"
#line 1943 "./usb_device.h"
#line 1944 "./usb_device.h"
#line 1945 "./usb_device.h"
#line 1946 "./usb_device.h"
#line 1947 "./usb_device.h"
#line 1948 "./usb_device.h"
#line 1949 "./usb_device.h"
#line 1950 "./usb_device.h"
#line 1951 "./usb_device.h"
#line 1952 "./usb_device.h"
#line 1953 "./usb_device.h"
#line 1954 "./usb_device.h"
#line 1955 "./usb_device.h"
#line 1956 "./usb_device.h"
#line 1957 "./usb_device.h"
#line 1958 "./usb_device.h"
#line 1959 "./usb_device.h"
#line 1960 "./usb_device.h"
#line 1961 "./usb_device.h"
#line 1962 "./usb_device.h"
#line 1963 "./usb_device.h"
#line 1964 "./usb_device.h"
#line 1965 "./usb_device.h"
#line 1966 "./usb_device.h"
#line 1967 "./usb_device.h"

#line 1969 "./usb_device.h"

#line 1971 "./usb_device.h"
#line 1972 "./usb_device.h"
#line 1973 "./usb_device.h"
#line 1975 "./usb_device.h"
#line 1977 "./usb_device.h"

#line 1979 "./usb_device.h"
#line 2051 "./usb_device.h"
#line 2053 "./usb_device.h"

extern volatile  BOOL RemoteWakeup;

#line 2057 "./usb_device.h"
#line 119 "./usb.h"
     
#line 121 "./usb.h"

#line 123 "./usb.h"
#line 125 "./usb.h"

#line 127 "./usb.h"
#line 129 "./usb.h"








#line 138 "./usb.h"
#line 139 "./usb.h"
#line 140 "./usb.h"


#line 143 "./usb.h"

#line 145 "./usb.h"
 

#line 60 "usb_host.c"



#line 1 "./HardwareProfile.h"

#line 49 "./HardwareProfile.h"
 


#line 53 "./HardwareProfile.h"



#line 57 "./HardwareProfile.h"
#line 58 "./HardwareProfile.h"
#line 1 "./HardwareProfile - PIC18F85J50TST.h"

#line 39 "./HardwareProfile - PIC18F85J50TST.h"
 


#line 43 "./HardwareProfile - PIC18F85J50TST.h"

     
     
     
    
    
    
    
    
    
    
    

     
    
    
    
    

    
#line 64 "./HardwareProfile - PIC18F85J50TST.h"
#line 65 "./HardwareProfile - PIC18F85J50TST.h"
#line 67 "./HardwareProfile - PIC18F85J50TST.h"
#line 68 "./HardwareProfile - PIC18F85J50TST.h"
#line 69 "./HardwareProfile - PIC18F85J50TST.h"

    
#line 72 "./HardwareProfile - PIC18F85J50TST.h"
#line 73 "./HardwareProfile - PIC18F85J50TST.h"
#line 75 "./HardwareProfile - PIC18F85J50TST.h"
#line 76 "./HardwareProfile - PIC18F85J50TST.h"
#line 77 "./HardwareProfile - PIC18F85J50TST.h"


   
    
    
#line 83 "./HardwareProfile - PIC18F85J50TST.h"

     
     
     
     
     
     
     

     
    
    
    
    
    
    
#line 100 "./HardwareProfile - PIC18F85J50TST.h"
#line 101 "./HardwareProfile - PIC18F85J50TST.h"
#line 102 "./HardwareProfile - PIC18F85J50TST.h"
  
	
#line 105 "./HardwareProfile - PIC18F85J50TST.h"
#line 106 "./HardwareProfile - PIC18F85J50TST.h"
#line 107 "./HardwareProfile - PIC18F85J50TST.h"
	
	
	
	
	
	
	
	
	
	
#line 118 "./HardwareProfile - PIC18F85J50TST.h"

	
	
	
	
	
	
	
	
	
	
	
#line 131 "./HardwareProfile - PIC18F85J50TST.h"

	
#line 134 "./HardwareProfile - PIC18F85J50TST.h"
#line 135 "./HardwareProfile - PIC18F85J50TST.h"
#line 136 "./HardwareProfile - PIC18F85J50TST.h"
#line 137 "./HardwareProfile - PIC18F85J50TST.h"
#line 138 "./HardwareProfile - PIC18F85J50TST.h"
	
	
	
	
	
	
	
	
	
	
#line 149 "./HardwareProfile - PIC18F85J50TST.h"

	
	
	
	
	
	
	
	
	
	
	
#line 162 "./HardwareProfile - PIC18F85J50TST.h"

	
#line 165 "./HardwareProfile - PIC18F85J50TST.h"
#line 166 "./HardwareProfile - PIC18F85J50TST.h"
#line 167 "./HardwareProfile - PIC18F85J50TST.h"
#line 168 "./HardwareProfile - PIC18F85J50TST.h"
#line 169 "./HardwareProfile - PIC18F85J50TST.h"
	
	
	
	
	
	
	
	
	
	
#line 180 "./HardwareProfile - PIC18F85J50TST.h"

	
#line 183 "./HardwareProfile - PIC18F85J50TST.h"
#line 184 "./HardwareProfile - PIC18F85J50TST.h"
#line 185 "./HardwareProfile - PIC18F85J50TST.h"
	
	
	
	
	
	
	
	
	
	
#line 196 "./HardwareProfile - PIC18F85J50TST.h"

	
#line 199 "./HardwareProfile - PIC18F85J50TST.h"
#line 200 "./HardwareProfile - PIC18F85J50TST.h"
#line 201 "./HardwareProfile - PIC18F85J50TST.h"
#line 202 "./HardwareProfile - PIC18F85J50TST.h"
	
	
	
	
	
	
	
	
	
	
#line 213 "./HardwareProfile - PIC18F85J50TST.h"

	
#line 216 "./HardwareProfile - PIC18F85J50TST.h"
#line 217 "./HardwareProfile - PIC18F85J50TST.h"
#line 218 "./HardwareProfile - PIC18F85J50TST.h"
#line 219 "./HardwareProfile - PIC18F85J50TST.h"
#line 220 "./HardwareProfile - PIC18F85J50TST.h"
	
	
	
	
	
	
	
	
	
	
#line 231 "./HardwareProfile - PIC18F85J50TST.h"

	
#line 234 "./HardwareProfile - PIC18F85J50TST.h"
#line 235 "./HardwareProfile - PIC18F85J50TST.h"
#line 236 "./HardwareProfile - PIC18F85J50TST.h"
	
	
	
	
	
	
	
	
	
	
#line 247 "./HardwareProfile - PIC18F85J50TST.h"

  
     
#line 251 "./HardwareProfile - PIC18F85J50TST.h"
#line 252 "./HardwareProfile - PIC18F85J50TST.h"


	
	
	
#line 258 "./HardwareProfile - PIC18F85J50TST.h"
#line 259 "./HardwareProfile - PIC18F85J50TST.h"
#line 260 "./HardwareProfile - PIC18F85J50TST.h"
#line 261 "./HardwareProfile - PIC18F85J50TST.h"
#line 262 "./HardwareProfile - PIC18F85J50TST.h"
	
#line 264 "./HardwareProfile - PIC18F85J50TST.h"
#line 265 "./HardwareProfile - PIC18F85J50TST.h"
#line 266 "./HardwareProfile - PIC18F85J50TST.h"

#line 268 "./HardwareProfile - PIC18F85J50TST.h"
#line 269 "./HardwareProfile - PIC18F85J50TST.h"

#line 271 "./HardwareProfile - PIC18F85J50TST.h"
#line 272 "./HardwareProfile - PIC18F85J50TST.h"


	
#line 276 "./HardwareProfile - PIC18F85J50TST.h"
#line 277 "./HardwareProfile - PIC18F85J50TST.h"
	
#line 279 "./HardwareProfile - PIC18F85J50TST.h"
#line 280 "./HardwareProfile - PIC18F85J50TST.h"

	
#line 283 "./HardwareProfile - PIC18F85J50TST.h"

	
	
	
#line 288 "./HardwareProfile - PIC18F85J50TST.h"
#line 289 "./HardwareProfile - PIC18F85J50TST.h"
#line 290 "./HardwareProfile - PIC18F85J50TST.h"
#line 291 "./HardwareProfile - PIC18F85J50TST.h"
#line 292 "./HardwareProfile - PIC18F85J50TST.h"

#line 294 "./HardwareProfile - PIC18F85J50TST.h"
#line 295 "./HardwareProfile - PIC18F85J50TST.h"
#line 296 "./HardwareProfile - PIC18F85J50TST.h"

#line 298 "./HardwareProfile - PIC18F85J50TST.h"
#line 58 "./HardwareProfile.h"

#line 60 "./HardwareProfile.h"
#line 61 "./HardwareProfile.h"

#line 63 "./HardwareProfile.h"
#line 65 "./HardwareProfile.h"

#line 67 "./HardwareProfile.h"
#line 63 "usb_host.c"


#line 66 "usb_host.c"
#line 68 "usb_host.c"





#line 76 "usb_host.c"




#line 81 "usb_host.c"




#line 86 "usb_host.c"



#line 90 "usb_host.c"

















#line 108 "usb_host.c"
#line 109 "usb_host.c"
#line 112 "usb_host.c"
#line 113 "usb_host.c"

#line 115 "usb_host.c"
#line 116 "usb_host.c"
#line 118 "usb_host.c"
#line 121 "usb_host.c"
#line 122 "usb_host.c"
#line 124 "usb_host.c"
#line 128 "usb_host.c"
#line 129 "usb_host.c"
    static BDT_ENTRY      BDT[4];
#line 131 "usb_host.c"
#line 132 "usb_host.c"
#line 133 "usb_host.c"
#line 134 "usb_host.c"
#line 135 "usb_host.c"
#line 136 "usb_host.c"

static BYTE                          countConfigurations;                        
static BYTE                          numCommandTries;                            
static BYTE                          numEnumerationTries;                        
static volatile WORD                 numTimerInterrupts;                         
static volatile USB_ENDPOINT_INFO   *pCurrentEndpoint;                           
static USB_CONFIGURATION            *pConfigurationDescriptorList       = 0 ;  
BYTE                                *pCurrentConfigurationDescriptor    = 0 ;  
BYTE                                *pDeviceDescriptor                  = 0 ;  
static USB_ENDPOINT_INFO            *pEndpointList                      = 0 ;  
static BYTE                         *pEP0Data                           = 0 ;  
static USB_INTERFACE_INFO           *pInterfaceList                     = 0 ;  
static USB_BUS_INFO                  usbBusInfo;                                 
static USB_DEVICE_INFO               usbDeviceInfo;                              
#line 151 "usb_host.c"
#line 153 "usb_host.c"
static volatile WORD                 usbHostState;                               
static volatile WORD                 usbOverrideHostState;                       
static USB_ROOT_HUB_INFO             usbRootHubInfo;                             

#line 160 "usb_host.c"









#line 195 "usb_host.c"
 

BYTE USBHostClearEndpointErrors( BYTE deviceAddress, BYTE endpoint )
{
    USB_ENDPOINT_INFO *ep;

    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    ep = pEndpointList;
    while (ep != 0 )
    {
        if (ep->bEndpointAddress == endpoint)
        {
            ep->status.bfStalled    = 0;
            ep->status.bfError      = 0;

            return 0x00 ;
        }
        ep = ep->next;
    }
    return 0x14 ;
}



#line 278 "usb_host.c"
 

BYTE USBHostDeviceRequest( BYTE deviceAddress, BYTE bmRequestType, BYTE bRequest,
            WORD wValue, WORD wIndex, WORD wLength, BYTE *data, BYTE dataDirection )
{
    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    
    if ((usbHostState & STATE_MASK) != STATE_RUNNING)
    {
        return 0x01 ;
    }

    
    if (!pEndpointList->status.bfTransferComplete)
    {
        return 0x10 ;
    }

    
    
    





    
    if (bRequest == 11 )
    {
        _USB_ResetDATA0( 0 );
    }

    
    if ((bRequest == 1 ) && (wValue == 0 ))
    {
        _USB_ResetDATA0( (BYTE)wIndex );
    }

    
    pEP0Data[0] = bmRequestType;
    pEP0Data[1] = bRequest;
    pEP0Data[2] = wValue & 0xFF;
    pEP0Data[3] = (wValue >> 8) & 0xFF;
    pEP0Data[4] = wIndex & 0xFF;
    pEP0Data[5] = (wIndex >> 8) & 0xFF;
    pEP0Data[6] = wLength & 0xFF;
    pEP0Data[7] = (wLength >> 8) & 0xFF;

    if (dataDirection == USB_DEVICE_REQUEST_SET)
    {
        
        _USB_InitControlWrite( pEndpointList, pEP0Data,8, data, wLength );
    }
    else
    {
        
        _USB_InitControlRead( pEndpointList, pEP0Data, 8, data, wLength );
    }

    return 0x00 ;
}


#line 389 "usb_host.c"
 

BOOL    USBHostDeviceSpecificClientDriver( BYTE deviceAddress )
{
    return usbDeviceInfo.flags.bfUseDeviceClientDriver;
}



#line 433 "usb_host.c"
 

BYTE USBHostDeviceStatus( BYTE deviceAddress )
{
    if ((usbHostState & STATE_MASK) == STATE_DETACHED)
    {
        return (0x30  | 0x01) ;
    }

    if ((usbHostState & STATE_MASK) == STATE_RUNNING)
    {
        if ((usbHostState & SUBSTATE_MASK) == SUBSTATE_SUSPEND_AND_RESUME)
        {
            return (0x30  | 0x0A) ;
        }
        else
        {
            return (0x30  | 0x30) ;
        }
    }

    if ((usbHostState & STATE_MASK) == STATE_HOLDING)
    {
        return usbDeviceInfo.errorCode;
    }

    return (0x30  | 0x02) ;
}


#line 490 "usb_host.c"
 

BOOL USBHostInit(  unsigned long flags  )
{
    
    
    
    if (pEndpointList == 0 )
    {
        if ((pEndpointList = (USB_ENDPOINT_INFO*)malloc( sizeof(USB_ENDPOINT_INFO) )) == 0 )
        {
#line 504 "usb_host.c"
            
            return FALSE;
        }
        pEndpointList->next = 0 ;
    }
    else
    {
        _USB_FreeMemory();
    }

    
    pCurrentEndpoint                        = pEndpointList;
    usbHostState                            = STATE_DETACHED;
    usbOverrideHostState                    = NO_STATE;
    usbDeviceInfo.deviceAddressAndSpeed     = 0;
    usbDeviceInfo.deviceAddress             = 0;
    usbRootHubInfo.flags.bPowerGoodPort0    = 1;

    
#line 524 "usb_host.c"
#line 526 "usb_host.c"

    return TRUE;
}


#line 562 "usb_host.c"
 

BYTE USBHostRead( BYTE deviceAddress, BYTE endpoint, BYTE *pData, DWORD size )
{
    USB_ENDPOINT_INFO *ep;

    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    
    if ((usbHostState & STATE_MASK) != STATE_RUNNING)
    {
        return 0x01 ;
    }

    ep = pEndpointList;
    while (ep != 0 )
    {
        if (ep->bEndpointAddress == endpoint)
        {
            if (ep->bmAttributes.bfTransferType == 0x00 )
            {
                
                return 0x18 ;
            }

            if (!(ep->bEndpointAddress & 0x80))
            {
                
                return 0x15 ;
            }

            if (ep->status.bfStalled)
            {
                
                
                return 0x11 ;
            }

            if (ep->status.bfError)
            {
                
                
                return 0x12 ;
            }

            if (!ep->status.bfTransferComplete)
            {
                
                return 0x10 ;
            }

            _USB_InitRead( ep, pData, size );

            return 0x00 ;
        }
        ep = ep->next;
    }
    return 0x14 ;   
}


#line 654 "usb_host.c"
 

BYTE USBHostResetDevice( BYTE deviceAddress )
{
    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    if ((usbHostState & STATE_MASK) == STATE_DETACHED)
    {
        return 0x03 ;
    }

    usbHostState = STATE_DETACHED;

    return 0x00 ;
}


#line 698 "usb_host.c"
 

BYTE USBHostResumeDevice( BYTE deviceAddress )
{
    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    if (usbHostState != (STATE_RUNNING | SUBSTATE_SUSPEND_AND_RESUME | SUBSUBSTATE_SUSPEND))
    {
        return 0x03 ;
    }

    
    _USB_SetNextSubSubState();

    return 0x00 ;
}


#line 753 "usb_host.c"
 

BYTE USBHostSetDeviceConfiguration( BYTE deviceAddress, BYTE configuration )
{
    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    
    if ((usbHostState & STATE_MASK) != STATE_RUNNING)
    {
        return 0x01 ;
    }

    
    if (_USB_TransferInProgress())
    {
        return 0x02 ;
    }

    
    usbDeviceInfo.currentConfiguration = configuration;

    
    
    
    
    
    usbHostState = STATE_CONFIGURING | SUBSTATE_SELECT_CONFIGURATION;

    return 0x00 ;
}



#line 820 "usb_host.c"
 

BYTE USBHostSetNAKTimeout( BYTE deviceAddress, BYTE endpoint, WORD flags, WORD timeoutCount )
{
    USB_ENDPOINT_INFO *ep;

    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    ep = pEndpointList;
    while (ep != 0 )
    {
        if (ep->bEndpointAddress == endpoint)
        {
            ep->status.bfNAKTimeoutEnabled  = flags & 0x01;
            ep->timeoutNAKs                 = timeoutCount;

            return 0x00 ;
        }
        ep = ep->next;
    }
    return 0x14 ;
}



#line 871 "usb_host.c"
 

void USBHostShutdown( void )
{
    
    

#line 898 "usb_host.c"
        U1PWRC = USB_NORMAL_OPERATION | USB_DISABLED;  

        
        
        if (usbDeviceInfo.deviceAddress)
        {
            USB_VBUS_POWER_EVENT_DATA   powerRequest;

            powerRequest.port = 0;  

            USB_HOST_APP_EVENT_HANDLER( usbDeviceInfo.deviceAddress, EVENT_VBUS_RELEASE_POWER,
                &powerRequest, sizeof(USB_VBUS_POWER_EVENT_DATA) );
            _USB_NotifyClients(usbDeviceInfo.deviceAddress, EVENT_DETACH,
                &usbDeviceInfo.deviceAddress, sizeof(BYTE) );


        }
#line 916 "usb_host.c"

    
    
    USBHostInit( 0 );
}



#line 948 "usb_host.c"
 

BYTE USBHostSuspendDevice( BYTE deviceAddress )
{
    
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return 0x06 ;
    }

    if (usbHostState != (STATE_RUNNING | SUBSTATE_NORMAL_RUN))
    {
        return 0x03 ;
    }

    
    UCONbits .SOFEN = 0;

    
    usbHostState = STATE_RUNNING | SUBSTATE_SUSPEND_AND_RESUME | SUBSUBSTATE_SUSPEND;

    return 0x00 ;
}


#line 1004 "usb_host.c"
 

void USBHostTasks( void )
{
    static USB_CONFIGURATION           *pCurrentConfigurationNode;  
    USB_INTERFACE_INFO          *pCurrentInterface;
    BYTE                        *pTemp;
    BYTE                        temp;
    USB_VBUS_POWER_EVENT_DATA   powerRequest;

#line 1022 "usb_host.c"

    
    
    
#line 1034 "usb_host.c"
#line 1037 "usb_host.c"
#line 1038 "usb_host.c"

    
#line 1041 "usb_host.c"
#line 1044 "usb_host.c"
#line 1046 "usb_host.c"
#line 1048 "usb_host.c"
#line 1050 "usb_host.c"
#line 1076 "usb_host.c"

    
    if (usbOverrideHostState != NO_STATE)
    {
#line 1083 "usb_host.c"
        usbHostState = usbOverrideHostState;
        usbOverrideHostState = NO_STATE;
    }

    
    

    switch (usbHostState & STATE_MASK)
    {
        case STATE_DETACHED:
            switch (usbHostState & SUBSTATE_MASK)
            {
                case SUBSTATE_INITIALIZE:
                    
                    

                    
                    USBHostShutdown();

#line 1105 "usb_host.c"

                    
                    pEndpointList->next                         = 0 ;
                    pEndpointList->status.val                   = 0x00;
                    pEndpointList->status.bfUseDTS              = 1;
                    pEndpointList->status.bfTransferComplete    = 1;    
                    pEndpointList->status.bfNAKTimeoutEnabled   = 1;    
                    pEndpointList->timeoutNAKs                  = USB_NUM_CONTROL_NAKS;
                    pEndpointList->wMaxPacketSize               = 64;
                    pEndpointList->dataCount                    = 0;    
                    pEndpointList->bEndpointAddress             = 0;
                    pEndpointList->transferState                = TSTATE_IDLE;
                    pEndpointList->bmAttributes.bfTransferType  = 0x00 ;
                    pEndpointList->pInterface                   = 0 ;

                    
                    numEnumerationTries                 = USB_NUM_ENUMERATION_TRIES;
                    usbDeviceInfo.currentConfiguration  = 0; 
                    usbDeviceInfo.attributesOTG         = 0;
                    usbDeviceInfo.deviceAddressAndSpeed = 0;
                    usbDeviceInfo.flags.val             = 0;

                    
                    UIE                 = 0;        
                    UIR                 = 0xFF;
                    U1OTGIE             &= 0x8C;
                    U1OTGIR             = 0x7D;
                    UEIE                = 0;
                    UEIR                = 0xFF;

                    
#line 1137 "usb_host.c"
#line 1139 "usb_host.c"
#line 1143 "usb_host.c"
