

using Gurux.DLMS;
using Gurux.DLMS.Enums;
using System;
using System.Collections.Generic;

namespace meterReader.AesGcmParameter
{
    internal class GXDLMSCipheringStream
    {
        private List<byte> Output = new List<byte>();
        private const int BlockSize = 16;
        private Security Security;
        private readonly uint[][][] M = new uint[32][][];
        private ulong totalLength;
        private static readonly byte[] Zeroes = new byte[16];
        private byte[] S;
        private byte[] counter;
        private byte[] Aad;
        private byte[] J0;
        private int BytesRemaining;
        private byte[] H;
        private byte[] bufBlock;
        private byte[] Tag;
        private int Rounds;
        private uint[,] WorkingKey;
        private uint C0;
        private uint C1;
        private uint C2;
        private uint C3;
        private bool Encrypt;
        private static readonly byte[] rcon = new byte[30]
        {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 8,
      (byte) 16,
      (byte) 32,
      (byte) 64,
      (byte) 128,
      (byte) 27,
      (byte) 54,
      (byte) 108,
      (byte) 216,
      (byte) 171,
      (byte) 77,
      (byte) 154,
      (byte) 47,
      (byte) 94,
      (byte) 188,
      (byte) 99,
      (byte) 198,
      (byte) 151,
      (byte) 53,
      (byte) 106,
      (byte) 212,
      (byte) 179,
      (byte) 125,
      (byte) 250,
      (byte) 239,
      (byte) 197,
      (byte) 145
        };
        private static readonly byte[] SBox = new byte[256]
        {
      (byte) 99,
      (byte) 124,
      (byte) 119,
      (byte) 123,
      (byte) 242,
      (byte) 107,
      (byte) 111,
      (byte) 197,
      (byte) 48,
      (byte) 1,
      (byte) 103,
      (byte) 43,
      (byte) 254,
      (byte) 215,
      (byte) 171,
      (byte) 118,
      (byte) 202,
      (byte) 130,
      (byte) 201,
      (byte) 125,
      (byte) 250,
      (byte) 89,
      (byte) 71,
      (byte) 240,
      (byte) 173,
      (byte) 212,
      (byte) 162,
      (byte) 175,
      (byte) 156,
      (byte) 164,
      (byte) 114,
      (byte) 192,
      (byte) 183,
      (byte) 253,
      (byte) 147,
      (byte) 38,
      (byte) 54,
      (byte) 63,
      (byte) 247,
      (byte) 204,
      (byte) 52,
      (byte) 165,
      (byte) 229,
      (byte) 241,
      (byte) 113,
      (byte) 216,
      (byte) 49,
      (byte) 21,
      (byte) 4,
      (byte) 199,
      (byte) 35,
      (byte) 195,
      (byte) 24,
      (byte) 150,
      (byte) 5,
      (byte) 154,
      (byte) 7,
      (byte) 18,
      (byte) 128,
      (byte) 226,
      (byte) 235,
      (byte) 39,
      (byte) 178,
      (byte) 117,
      (byte) 9,
      (byte) 131,
      (byte) 44,
      (byte) 26,
      (byte) 27,
      (byte) 110,
      (byte) 90,
      (byte) 160,
      (byte) 82,
      (byte) 59,
      (byte) 214,
      (byte) 179,
      (byte) 41,
      (byte) 227,
      (byte) 47,
      (byte) 132,
      (byte) 83,
      (byte) 209,
      (byte) 0,
      (byte) 237,
      (byte) 32,
      (byte) 252,
      (byte) 177,
      (byte) 91,
      (byte) 106,
      (byte) 203,
      (byte) 190,
      (byte) 57,
      (byte) 74,
      (byte) 76,
      (byte) 88,
      (byte) 207,
      (byte) 208,
      (byte) 239,
      (byte) 170,
      (byte) 251,
      (byte) 67,
      (byte) 77,
      (byte) 51,
      (byte) 133,
      (byte) 69,
      (byte) 249,
      (byte) 2,
      (byte) 127,
      (byte) 80,
      (byte) 60,
      (byte) 159,
      (byte) 168,
      (byte) 81,
      (byte) 163,
      (byte) 64,
      (byte) 143,
      (byte) 146,
      (byte) 157,
      (byte) 56,
      (byte) 245,
      (byte) 188,
      (byte) 182,
      (byte) 218,
      (byte) 33,
      (byte) 16,
      byte.MaxValue,
      (byte) 243,
      (byte) 210,
      (byte) 205,
      (byte) 12,
      (byte) 19,
      (byte) 236,
      (byte) 95,
      (byte) 151,
      (byte) 68,
      (byte) 23,
      (byte) 196,
      (byte) 167,
      (byte) 126,
      (byte) 61,
      (byte) 100,
      (byte) 93,
      (byte) 25,
      (byte) 115,
      (byte) 96,
      (byte) 129,
      (byte) 79,
      (byte) 220,
      (byte) 34,
      (byte) 42,
      (byte) 144,
      (byte) 136,
      (byte) 70,
      (byte) 238,
      (byte) 184,
      (byte) 20,
      (byte) 222,
      (byte) 94,
      (byte) 11,
      (byte) 219,
      (byte) 224,
      (byte) 50,
      (byte) 58,
      (byte) 10,
      (byte) 73,
      (byte) 6,
      (byte) 36,
      (byte) 92,
      (byte) 194,
      (byte) 211,
      (byte) 172,
      (byte) 98,
      (byte) 145,
      (byte) 149,
      (byte) 228,
      (byte) 121,
      (byte) 231,
      (byte) 200,
      (byte) 55,
      (byte) 109,
      (byte) 141,
      (byte) 213,
      (byte) 78,
      (byte) 169,
      (byte) 108,
      (byte) 86,
      (byte) 244,
      (byte) 234,
      (byte) 101,
      (byte) 122,
      (byte) 174,
      (byte) 8,
      (byte) 186,
      (byte) 120,
      (byte) 37,
      (byte) 46,
      (byte) 28,
      (byte) 166,
      (byte) 180,
      (byte) 198,
      (byte) 232,
      (byte) 221,
      (byte) 116,
      (byte) 31,
      (byte) 75,
      (byte) 189,
      (byte) 139,
      (byte) 138,
      (byte) 112,
      (byte) 62,
      (byte) 181,
      (byte) 102,
      (byte) 72,
      (byte) 3,
      (byte) 246,
      (byte) 14,
      (byte) 97,
      (byte) 53,
      (byte) 87,
      (byte) 185,
      (byte) 134,
      (byte) 193,
      (byte) 29,
      (byte) 158,
      (byte) 225,
      (byte) 248,
      (byte) 152,
      (byte) 17,
      (byte) 105,
      (byte) 217,
      (byte) 142,
      (byte) 148,
      (byte) 155,
      (byte) 30,
      (byte) 135,
      (byte) 233,
      (byte) 206,
      (byte) 85,
      (byte) 40,
      (byte) 223,
      (byte) 140,
      (byte) 161,
      (byte) 137,
      (byte) 13,
      (byte) 191,
      (byte) 230,
      (byte) 66,
      (byte) 104,
      (byte) 65,
      (byte) 153,
      (byte) 45,
      (byte) 15,
      (byte) 176,
      (byte) 84,
      (byte) 187,
      (byte) 22
        };
        private static readonly byte[] SBoxInverse = new byte[256]
        {
      (byte) 82,
      (byte) 9,
      (byte) 106,
      (byte) 213,
      (byte) 48,
      (byte) 54,
      (byte) 165,
      (byte) 56,
      (byte) 191,
      (byte) 64,
      (byte) 163,
      (byte) 158,
      (byte) 129,
      (byte) 243,
      (byte) 215,
      (byte) 251,
      (byte) 124,
      (byte) 227,
      (byte) 57,
      (byte) 130,
      (byte) 155,
      (byte) 47,
      byte.MaxValue,
      (byte) 135,
      (byte) 52,
      (byte) 142,
      (byte) 67,
      (byte) 68,
      (byte) 196,
      (byte) 222,
      (byte) 233,
      (byte) 203,
      (byte) 84,
      (byte) 123,
      (byte) 148,
      (byte) 50,
      (byte) 166,
      (byte) 194,
      (byte) 35,
      (byte) 61,
      (byte) 238,
      (byte) 76,
      (byte) 149,
      (byte) 11,
      (byte) 66,
      (byte) 250,
      (byte) 195,
      (byte) 78,
      (byte) 8,
      (byte) 46,
      (byte) 161,
      (byte) 102,
      (byte) 40,
      (byte) 217,
      (byte) 36,
      (byte) 178,
      (byte) 118,
      (byte) 91,
      (byte) 162,
      (byte) 73,
      (byte) 109,
      (byte) 139,
      (byte) 209,
      (byte) 37,
      (byte) 114,
      (byte) 248,
      (byte) 246,
      (byte) 100,
      (byte) 134,
      (byte) 104,
      (byte) 152,
      (byte) 22,
      (byte) 212,
      (byte) 164,
      (byte) 92,
      (byte) 204,
      (byte) 93,
      (byte) 101,
      (byte) 182,
      (byte) 146,
      (byte) 108,
      (byte) 112,
      (byte) 72,
      (byte) 80,
      (byte) 253,
      (byte) 237,
      (byte) 185,
      (byte) 218,
      (byte) 94,
      (byte) 21,
      (byte) 70,
      (byte) 87,
      (byte) 167,
      (byte) 141,
      (byte) 157,
      (byte) 132,
      (byte) 144,
      (byte) 216,
      (byte) 171,
      (byte) 0,
      (byte) 140,
      (byte) 188,
      (byte) 211,
      (byte) 10,
      (byte) 247,
      (byte) 228,
      (byte) 88,
      (byte) 5,
      (byte) 184,
      (byte) 179,
      (byte) 69,
      (byte) 6,
      (byte) 208,
      (byte) 44,
      (byte) 30,
      (byte) 143,
      (byte) 202,
      (byte) 63,
      (byte) 15,
      (byte) 2,
      (byte) 193,
      (byte) 175,
      (byte) 189,
      (byte) 3,
      (byte) 1,
      (byte) 19,
      (byte) 138,
      (byte) 107,
      (byte) 58,
      (byte) 145,
      (byte) 17,
      (byte) 65,
      (byte) 79,
      (byte) 103,
      (byte) 220,
      (byte) 234,
      (byte) 151,
      (byte) 242,
      (byte) 207,
      (byte) 206,
      (byte) 240,
      (byte) 180,
      (byte) 230,
      (byte) 115,
      (byte) 150,
      (byte) 172,
      (byte) 116,
      (byte) 34,
      (byte) 231,
      (byte) 173,
      (byte) 53,
      (byte) 133,
      (byte) 226,
      (byte) 249,
      (byte) 55,
      (byte) 232,
      (byte) 28,
      (byte) 117,
      (byte) 223,
      (byte) 110,
      (byte) 71,
      (byte) 241,
      (byte) 26,
      (byte) 113,
      (byte) 29,
      (byte) 41,
      (byte) 197,
      (byte) 137,
      (byte) 111,
      (byte) 183,
      (byte) 98,
      (byte) 14,
      (byte) 170,
      (byte) 24,
      (byte) 190,
      (byte) 27,
      (byte) 252,
      (byte) 86,
      (byte) 62,
      (byte) 75,
      (byte) 198,
      (byte) 210,
      (byte) 121,
      (byte) 32,
      (byte) 154,
      (byte) 219,
      (byte) 192,
      (byte) 254,
      (byte) 120,
      (byte) 205,
      (byte) 90,
      (byte) 244,
      (byte) 31,
      (byte) 221,
      (byte) 168,
      (byte) 51,
      (byte) 136,
      (byte) 7,
      (byte) 199,
      (byte) 49,
      (byte) 177,
      (byte) 18,
      (byte) 16,
      (byte) 89,
      (byte) 39,
      (byte) 128,
      (byte) 236,
      (byte) 95,
      (byte) 96,
      (byte) 81,
      (byte) 127,
      (byte) 169,
      (byte) 25,
      (byte) 181,
      (byte) 74,
      (byte) 13,
      (byte) 45,
      (byte) 229,
      (byte) 122,
      (byte) 159,
      (byte) 147,
      (byte) 201,
      (byte) 156,
      (byte) 239,
      (byte) 160,
      (byte) 224,
      (byte) 59,
      (byte) 77,
      (byte) 174,
      (byte) 42,
      (byte) 245,
      (byte) 176,
      (byte) 200,
      (byte) 235,
      (byte) 187,
      (byte) 60,
      (byte) 131,
      (byte) 83,
      (byte) 153,
      (byte) 97,
      (byte) 23,
      (byte) 43,
      (byte) 4,
      (byte) 126,
      (byte) 186,
      (byte) 119,
      (byte) 214,
      (byte) 38,
      (byte) 225,
      (byte) 105,
      (byte) 20,
      (byte) 99,
      (byte) 85,
      (byte) 33,
      (byte) 12,
      (byte) 125
        };
        private static readonly uint[] AES = new uint[256]
        {
      2774754246U,
      2222750968U,
      2574743534U,
      2373680118U,
      234025727U,
      3177933782U,
      2976870366U,
      1422247313U,
      1345335392U,
      50397442U,
      2842126286U,
      2099981142U,
      436141799U,
      1658312629U,
      3870010189U,
      2591454956U,
      1170918031U,
      2642575903U,
      1086966153U,
      2273148410U,
      368769775U,
      3948501426U,
      3376891790U,
      200339707U,
      3970805057U,
      1742001331U,
      4255294047U,
      3937382213U,
      3214711843U,
      4154762323U,
      2524082916U,
      1539358875U,
      3266819957U,
      486407649U,
      2928907069U,
      1780885068U,
      1513502316U,
      1094664062U,
      49805301U,
      1338821763U,
      1546925160U,
      4104496465U,
      887481809U,
      150073849U,
      2473685474U,
      1943591083U,
      1395732834U,
      1058346282U,
      201589768U,
      1388824469U,
      1696801606U,
      1589887901U,
      672667696U,
      2711000631U,
      251987210U,
      3046808111U,
      151455502U,
      907153956U,
      2608889883U,
      1038279391U,
      652995533U,
      1764173646U,
      3451040383U,
      2675275242U,
      453576978U,
      2659418909U,
      1949051992U,
      773462580U,
      756751158U,
      2993581788U,
      3998898868U,
      4221608027U,
      4132590244U,
      1295727478U,
      1641469623U,
      3467883389U,
      2066295122U,
      1055122397U,
      1898917726U,
      2542044179U,
      4115878822U,
      1758581177U,
      0U,
      753790401U,
      1612718144U,
      536673507U,
      3367088505U,
      3982187446U,
      3194645204U,
      1187761037U,
      3653156455U,
      1262041458U,
      3729410708U,
      3561770136U,
      3898103984U,
      1255133061U,
      1808847035U,
      720367557U,
      3853167183U,
      385612781U,
      3309519750U,
      3612167578U,
      1429418854U,
      2491778321U,
      3477423498U,
      284817897U,
      100794884U,
      2172616702U,
      4031795360U,
      1144798328U,
      3131023141U,
      3819481163U,
      4082192802U,
      4272137053U,
      3225436288U,
      2324664069U,
      2912064063U,
      3164445985U,
      1211644016U,
      83228145U,
      3753688163U,
      3249976951U,
      1977277103U,
      1663115586U,
      806359072U,
      452984805U,
      250868733U,
      1842533055U,
      1288555905U,
      336333848U,
      890442534U,
      804056259U,
      3781124030U,
      2727843637U,
      3427026056U,
      957814574U,
      1472513171U,
      4071073621U,
      2189328124U,
      1195195770U,
      2892260552U,
      3881655738U,
      723065138U,
      2507371494U,
      2690670784U,
      2558624025U,
      3511635870U,
      2145180835U,
      1713513028U,
      2116692564U,
      2878378043U,
      2206763019U,
      3393603212U,
      703524551U,
      3552098411U,
      1007948840U,
      2044649127U,
      3797835452U,
      487262998U,
      1994120109U,
      1004593371U,
      1446130276U,
      1312438900U,
      503974420U,
      3679013266U,
      168166924U,
      1814307912U,
      3831258296U,
      1573044895U,
      1859376061U,
      4021070915U,
      2791465668U,
      2828112185U,
      2761266481U,
      937747667U,
      2339994098U,
      854058965U,
      1137232011U,
      1496790894U,
      3077402074U,
      2358086913U,
      1691735473U,
      3528347292U,
      3769215305U,
      3027004632U,
      4199962284U,
      133494003U,
      636152527U,
      2942657994U,
      2390391540U,
      3920539207U,
      403179536U,
      3585784431U,
      2289596656U,
      1864705354U,
      1915629148U,
      605822008U,
      4054230615U,
      3350508659U,
      1371981463U,
      602466507U,
      2094914977U,
      2624877800U,
      555687742U,
      3712699286U,
      3703422305U,
      2257292045U,
      2240449039U,
      2423288032U,
      1111375484U,
      3300242801U,
      2858837708U,
      3628615824U,
      84083462U,
      32962295U,
      302911004U,
      2741068226U,
      1597322602U,
      4183250862U,
      3501832553U,
      2441512471U,
      1489093017U,
      656219450U,
      3114180135U,
      954327513U,
      335083755U,
      3013122091U,
      856756514U,
      3144247762U,
      1893325225U,
      2307821063U,
      2811532339U,
      3063651117U,
      572399164U,
      2458355477U,
      552200649U,
      1238290055U,
      4283782570U,
      2015897680U,
      2061492133U,
      2408352771U,
      4171342169U,
      2156497161U,
      386731290U,
      3669999461U,
      837215959U,
      3326231172U,
      3093850320U,
      3275833730U,
      2962856233U,
      1999449434U,
      286199582U,
      3417354363U,
      4233385128U,
      3602627437U,
      974525996U
        };
        private static readonly uint[] Reversed_AES1 = new uint[256]
        {
      1353184337U,
      1399144830U,
      3282310938U,
      2522752826U,
      3412831035U,
      4047871263U,
      2874735276U,
      2466505547U,
      1442459680U,
      4134368941U,
      2440481928U,
      625738485U,
      4242007375U,
      3620416197U,
      2151953702U,
      2409849525U,
      1230680542U,
      1729870373U,
      2551114309U,
      3787521629U,
      41234371U,
      317738113U,
      2744600205U,
      3338261355U,
      3881799427U,
      2510066197U,
      3950669247U,
      3663286933U,
      763608788U,
      3542185048U,
      694804553U,
      1154009486U,
      1787413109U,
      2021232372U,
      1799248025U,
      3715217703U,
      3058688446U,
      397248752U,
      1722556617U,
      3023752829U,
      407560035U,
      2184256229U,
      1613975959U,
      1165972322U,
      3765920945U,
      2226023355U,
      480281086U,
      2485848313U,
      1483229296U,
      436028815U,
      2272059028U,
      3086515026U,
      601060267U,
      3791801202U,
      1468997603U,
      715871590U,
      120122290U,
      63092015U,
      2591802758U,
      2768779219U,
      4068943920U,
      2997206819U,
      3127509762U,
      1552029421U,
      723308426U,
      2461301159U,
      4042393587U,
      2715969870U,
      3455375973U,
      3586000134U,
      526529745U,
      2331944644U,
      2639474228U,
      2689987490U,
      853641733U,
      1978398372U,
      971801355U,
      2867814464U,
      111112542U,
      1360031421U,
      4186579262U,
      1023860118U,
      2919579357U,
      1186850381U,
      3045938321U,
      90031217U,
      1876166148U,
      4279586912U,
      620468249U,
      2548678102U,
      3426959497U,
      2006899047U,
      3175278768U,
      2290845959U,
      945494503U,
      3689859193U,
      1191869601U,
      3910091388U,
      3374220536U,
      0U,
      2206629897U,
      1223502642U,
      2893025566U,
      1316117100U,
      4227796733U,
      1446544655U,
      517320253U,
      658058550U,
      1691946762U,
      564550760U,
      3511966619U,
      976107044U,
      2976320012U,
      266819475U,
      3533106868U,
      2660342555U,
      1338359936U,
      2720062561U,
      1766553434U,
      370807324U,
      179999714U,
      3844776128U,
      1138762300U,
      488053522U,
      185403662U,
      2915535858U,
      3114841645U,
      3366526484U,
      2233069911U,
      1275557295U,
      3151862254U,
      4250959779U,
      2670068215U,
      3170202204U,
      3309004356U,
      880737115U,
      1982415755U,
      3703972811U,
      1761406390U,
      1676797112U,
      3403428311U,
      277177154U,
      1076008723U,
      538035844U,
      2099530373U,
      4164795346U,
      288553390U,
      1839278535U,
      1261411869U,
      4080055004U,
      3964831245U,
      3504587127U,
      1813426987U,
      2579067049U,
      4199060497U,
      577038663U,
      3297574056U,
      440397984U,
      3626794326U,
      4019204898U,
      3343796615U,
      3251714265U,
      4272081548U,
      906744984U,
      3481400742U,
      685669029U,
      646887386U,
      2764025151U,
      3835509292U,
      227702864U,
      2613862250U,
      1648787028U,
      3256061430U,
      3904428176U,
      1593260334U,
      4121936770U,
      3196083615U,
      2090061929U,
      2838353263U,
      3004310991U,
      999926984U,
      2809993232U,
      1852021992U,
      2075868123U,
      158869197U,
      4095236462U,
      28809964U,
      2828685187U,
      1701746150U,
      2129067946U,
      147831841U,
      3873969647U,
      3650873274U,
      3459673930U,
      3557400554U,
      3598495785U,
      2947720241U,
      824393514U,
      815048134U,
      3227951669U,
      935087732U,
      2798289660U,
      2966458592U,
      366520115U,
      1251476721U,
      4158319681U,
      240176511U,
      804688151U,
      2379631990U,
      1303441219U,
      1414376140U,
      3741619940U,
      3820343710U,
      461924940U,
      3089050817U,
      2136040774U,
      82468509U,
      1563790337U,
      1937016826U,
      776014843U,
      1511876531U,
      1389550482U,
      861278441U,
      323475053U,
      2355222426U,
      2047648055U,
      2383738969U,
      2302415851U,
      3995576782U,
      902390199U,
      3991215329U,
      1018251130U,
      1507840668U,
      1064563285U,
      2043548696U,
      3208103795U,
      3939366739U,
      1537932639U,
      342834655U,
      2262516856U,
      2180231114U,
      1053059257U,
      741614648U,
      1598071746U,
      1925389590U,
      203809468U,
      2336832552U,
      1100287487U,
      1895934009U,
      3736275976U,
      2632234200U,
      2428589668U,
      1636092795U,
      1890988757U,
      1952214088U,
      1113045200U
        };

        public GXDLMSCipheringStream(bool encrypt, byte[] kek)
        {
            this.Encrypt = encrypt;
            this.WorkingKey = this.GenerateKey(encrypt, kek);
        }

        public GXDLMSCipheringStream(
          Security security,
          bool encrypt,
          byte[] blockCipherKey,
          byte[] aad,
          byte[] iv,
          byte[] tag)
        {
            this.Security = security;
            this.Tag = tag;
            if (this.Tag == null)
                this.Tag = new byte[12];
            else if (this.Tag.Length != 12)
                throw new ArgumentOutOfRangeException("Invalid tag.");
            this.Encrypt = encrypt;
            this.WorkingKey = this.GenerateKey(encrypt, blockCipherKey);
            this.bufBlock = new byte[this.Encrypt ? 16 : 32];
            this.Aad = aad;
            this.H = new byte[16];
            this.ProcessBlock(this.H, 0, this.H, 0);
            this.Init(this.H);
            this.J0 = new byte[16];
            Array.Copy((Array)iv, 0, (Array)this.J0, 0, iv.Length);
            this.J0[15] = (byte)1;
            this.S = this.GetGHash(this.Aad);
            this.counter = (byte[])this.J0.Clone();
            this.BytesRemaining = 0;
            this.totalLength = 0UL;
        }

        public virtual byte[] GetTag() => this.Tag;

        private static uint ToUInt32(byte[] value, int offset)
        {
            return (uint)((int)value[offset] | (int)value[++offset] << 8 | (int)value[++offset] << 16 | (int)value[++offset] << 24);
        }

        private static uint SubWord(uint value)
        {
            return (uint)((int)GXDLMSCipheringStream.SBox[(int)value & (int)byte.MaxValue] | (int)GXDLMSCipheringStream.SBox[(int)(value >> 8) & (int)byte.MaxValue] << 8 | (int)GXDLMSCipheringStream.SBox[(int)(value >> 16) & (int)byte.MaxValue] << 16 | (int)GXDLMSCipheringStream.SBox[(int)(value >> 24) & (int)byte.MaxValue] << 24);
        }

        private uint Shift(uint value, int shift) => value >> shift | value << 32 - shift;

        private uint StarX(uint value)
        {
            return (uint)(((int)value & 2139062143) << 1 ^ (int)((value & 2155905152U) >> 7) * 27);
        }

        private uint ImixCol(uint x)
        {
            uint num1 = this.StarX(x);
            uint num2 = this.StarX(num1);
            uint num3 = this.StarX(num2);
            uint num4 = x ^ num3;
            return num1 ^ num2 ^ num3 ^ this.Shift(num1 ^ num4, 8) ^ this.Shift(num2 ^ num4, 16) ^ this.Shift(num4, 24);
        }

        internal static void GetUInt32(uint value, byte[] data, int offset)
        {
            data[offset] = (byte)value;
            data[++offset] = (byte)(value >> 8);
            data[++offset] = (byte)(value >> 16);
            data[++offset] = (byte)(value >> 24);
        }

        private void UnPackBlock(byte[] bytes, int offset)
        {
            this.C0 = GXDLMSCipheringStream.ToUInt32(bytes, offset);
            this.C1 = GXDLMSCipheringStream.ToUInt32(bytes, offset + 4);
            this.C2 = GXDLMSCipheringStream.ToUInt32(bytes, offset + 8);
            this.C3 = GXDLMSCipheringStream.ToUInt32(bytes, offset + 12);
        }

        private void PackBlock(byte[] bytes, int offset)
        {
            GXDLMSCipheringStream.GetUInt32(this.C0, bytes, offset);
            GXDLMSCipheringStream.GetUInt32(this.C1, bytes, offset + 4);
            GXDLMSCipheringStream.GetUInt32(this.C2, bytes, offset + 8);
            GXDLMSCipheringStream.GetUInt32(this.C3, bytes, offset + 12);
        }

        private void EncryptBlock(uint[,] key)
        {
            this.C0 ^= key[0, 0];
            this.C1 ^= key[0, 1];
            this.C2 ^= key[0, 2];
            this.C3 ^= key[0, 3];
            int index1 = 1;
            while (index1 < this.Rounds - 1)
            {
                uint num1 = GXDLMSCipheringStream.AES[(int)this.C0 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C1 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C2 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C3 >> 24)], 8) ^ key[index1, 0];
                uint num2 = GXDLMSCipheringStream.AES[(int)this.C1 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C2 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C3 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C0 >> 24)], 8) ^ key[index1, 1];
                uint num3 = GXDLMSCipheringStream.AES[(int)this.C2 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C3 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C0 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C1 >> 24)], 8) ^ key[index1, 2];
                int num4 = (int)GXDLMSCipheringStream.AES[(int)this.C3 & (int)byte.MaxValue] ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(this.C0 >> 8) & (int)byte.MaxValue], 24) ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(this.C1 >> 16) & (int)byte.MaxValue], 16) ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(this.C2 >> 24)], 8);
                uint[,] numArray1 = key;
                int index2 = index1;
                int index3 = index2 + 1;
                int num5 = (int)numArray1[index2, 3];
                uint num6 = (uint)(num4 ^ num5);
                this.C0 = GXDLMSCipheringStream.AES[(int)num1 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num2 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num3 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num6 >> 24)], 8) ^ key[index3, 0];
                this.C1 = GXDLMSCipheringStream.AES[(int)num2 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num3 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num6 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num1 >> 24)], 8) ^ key[index3, 1];
                this.C2 = GXDLMSCipheringStream.AES[(int)num3 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num6 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num1 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(num2 >> 24)], 8) ^ key[index3, 2];
                int num7 = (int)GXDLMSCipheringStream.AES[(int)num6 & (int)byte.MaxValue] ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(num1 >> 8) & (int)byte.MaxValue], 24) ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(num2 >> 16) & (int)byte.MaxValue], 16) ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(num3 >> 24)], 8);
                uint[,] numArray2 = key;
                int index4 = index3;
                index1 = index4 + 1;
                int num8 = (int)numArray2[index4, 3];
                this.C3 = (uint)(num7 ^ num8);
            }
            uint num9 = GXDLMSCipheringStream.AES[(int)this.C0 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C1 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C2 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C3 >> 24)], 8) ^ key[index1, 0];
            uint num10 = GXDLMSCipheringStream.AES[(int)this.C1 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C2 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C3 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C0 >> 24)], 8) ^ key[index1, 1];
            uint num11 = GXDLMSCipheringStream.AES[(int)this.C2 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C3 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C0 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.AES[(int)(this.C1 >> 24)], 8) ^ key[index1, 2];
            int num12 = (int)GXDLMSCipheringStream.AES[(int)this.C3 & (int)byte.MaxValue] ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(this.C0 >> 8) & (int)byte.MaxValue], 24) ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(this.C1 >> 16) & (int)byte.MaxValue], 16) ^ (int)this.Shift(GXDLMSCipheringStream.AES[(int)(this.C2 >> 24)], 8);
            uint[,] numArray = key;
            int index5 = index1;
            int index6 = index5 + 1;
            int num13 = (int)numArray[index5, 3];
            uint num14 = (uint)(num12 ^ num13);
            this.C0 = (uint)((int)GXDLMSCipheringStream.SBox[(int)num9 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBox[(int)(num10 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBox[(int)(num11 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBox[(int)(num14 >> 24)] << 24) ^ key[index6, 0];
            this.C1 = (uint)((int)GXDLMSCipheringStream.SBox[(int)num10 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBox[(int)(num11 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBox[(int)(num14 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBox[(int)(num9 >> 24)] << 24) ^ key[index6, 1];
            this.C2 = (uint)((int)GXDLMSCipheringStream.SBox[(int)num11 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBox[(int)(num14 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBox[(int)(num9 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBox[(int)(num10 >> 24)] << 24) ^ key[index6, 2];
            this.C3 = (uint)((int)GXDLMSCipheringStream.SBox[(int)num14 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBox[(int)(num9 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBox[(int)(num10 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBox[(int)(num11 >> 24)] << 24) ^ key[index6, 3];
        }

        private void DecryptBlock(uint[,] key)
        {
            uint num1 = this.C0 ^ key[this.Rounds, 0];
            uint num2 = this.C1 ^ key[this.Rounds, 1];
            uint num3 = this.C2 ^ key[this.Rounds, 2];
            uint num4 = this.C3 ^ key[this.Rounds, 3];
            int index1;
            for (int index2 = this.Rounds - 1; index2 > 1; index2 = index1 - 1)
            {
                uint num5 = GXDLMSCipheringStream.Reversed_AES1[(int)num1 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num4 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num3 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num2 >> 24) & (int)byte.MaxValue], 8) ^ key[index2, 0];
                uint num6 = GXDLMSCipheringStream.Reversed_AES1[(int)num2 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num1 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num4 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num3 >> 24) & (int)byte.MaxValue], 8) ^ key[index2, 1];
                uint num7 = GXDLMSCipheringStream.Reversed_AES1[(int)num3 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num2 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num1 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num4 >> 24) & (int)byte.MaxValue], 8) ^ key[index2, 2];
                uint num8 = GXDLMSCipheringStream.Reversed_AES1[(int)num4 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num3 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num2 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num1 >> 24) & (int)byte.MaxValue], 8) ^ key[index2, 3];
                index1 = index2 - 1;
                num1 = GXDLMSCipheringStream.Reversed_AES1[(int)num5 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num8 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num7 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num6 >> 24) & (int)byte.MaxValue], 8) ^ key[index1, 0];
                num2 = GXDLMSCipheringStream.Reversed_AES1[(int)num6 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num5 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num8 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num7 >> 24) & (int)byte.MaxValue], 8) ^ key[index1, 1];
                num3 = GXDLMSCipheringStream.Reversed_AES1[(int)num7 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num6 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num5 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num8 >> 24) & (int)byte.MaxValue], 8) ^ key[index1, 2];
                num4 = GXDLMSCipheringStream.Reversed_AES1[(int)num8 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num7 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num6 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num5 >> 24) & (int)byte.MaxValue], 8) ^ key[index1, 3];
            }
            int index3 = 1;
            uint num9 = GXDLMSCipheringStream.Reversed_AES1[(int)num1 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num4 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num3 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num2 >> 24) & (int)byte.MaxValue], 8) ^ key[index3, 0];
            uint num10 = GXDLMSCipheringStream.Reversed_AES1[(int)num2 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num1 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num4 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num3 >> 24) & (int)byte.MaxValue], 8) ^ key[index3, 1];
            uint num11 = GXDLMSCipheringStream.Reversed_AES1[(int)num3 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num2 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num1 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num4 >> 24) & (int)byte.MaxValue], 8) ^ key[index3, 2];
            uint num12 = GXDLMSCipheringStream.Reversed_AES1[(int)num4 & (int)byte.MaxValue] ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num3 >> 8) & (int)byte.MaxValue], 24) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num2 >> 16) & (int)byte.MaxValue], 16) ^ this.Shift(GXDLMSCipheringStream.Reversed_AES1[(int)(num1 >> 24) & (int)byte.MaxValue], 8) ^ key[index3, 3];
            int index4 = 0;
            this.C0 = (uint)((int)GXDLMSCipheringStream.SBoxInverse[(int)num9 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num12 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num11 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num10 >> 24) & (int)byte.MaxValue] << 24) ^ key[index4, 0];
            this.C1 = (uint)((int)GXDLMSCipheringStream.SBoxInverse[(int)num10 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num9 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num12 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num11 >> 24) & (int)byte.MaxValue] << 24) ^ key[index4, 1];
            this.C2 = (uint)((int)GXDLMSCipheringStream.SBoxInverse[(int)num11 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num10 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num9 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num12 >> 24) & (int)byte.MaxValue] << 24) ^ key[index4, 2];
            this.C3 = (uint)((int)GXDLMSCipheringStream.SBoxInverse[(int)num12 & (int)byte.MaxValue] ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num11 >> 8) & (int)byte.MaxValue] << 8 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num10 >> 16) & (int)byte.MaxValue] << 16 ^ (int)GXDLMSCipheringStream.SBoxInverse[(int)(num9 >> 24) & (int)byte.MaxValue] << 24) ^ key[index4, 3];
        }

        private int ProcessBlock(byte[] input, int inOffset, byte[] output, int outOffset)
        {
            if (inOffset + 16 > input.Length)
                throw new ArgumentOutOfRangeException("Invalid input buffer.");
            if (outOffset + 16 > output.Length)
                throw new ArgumentOutOfRangeException("Invalid output buffer.");
            this.UnPackBlock(input, inOffset);
            if (this.Encrypt)
                this.EncryptBlock(this.WorkingKey);
            else
                this.DecryptBlock(this.WorkingKey);
            this.PackBlock(output, outOffset);
            return 16;
        }

        internal static uint BEToUInt32(byte[] buff, int offset)
        {
            return (uint)((int)buff[offset] << 24 | (int)buff[++offset] << 16 | (int)buff[++offset] << 8) | (uint)buff[++offset];
        }

        internal static void ShiftRight(uint[] block, int count)
        {
            uint num1 = 0;
            for (int index = 0; index < 4; ++index)
            {
                uint num2 = block[index];
                block[index] = num2 >> count | num1;
                num1 = num2 << 32 - count;
            }
        }

        internal static void MultiplyP(uint[] x)
        {
            bool flag = (x[3] & 1U) > 0U;
            GXDLMSCipheringStream.ShiftRight(x, 1);
            if (!flag)
                return;
            x[0] ^= 3774873600U;
        }

        internal static uint[] GetUint128(byte[] buff)
        {
            return new uint[4]
            {
        GXDLMSCipheringStream.BEToUInt32(buff, 0),
        GXDLMSCipheringStream.BEToUInt32(buff, 4),
        GXDLMSCipheringStream.BEToUInt32(buff, 8),
        GXDLMSCipheringStream.BEToUInt32(buff, 12)
            };
        }

        private static void Xor(byte[] block, byte[] value)
        {
            for (int index = 0; index != 16; ++index)
                block[index] ^= value[index];
        }

        private static void Xor(uint[] block, uint[] value)
        {
            for (int index = 0; index != 4; ++index)
                block[index] ^= value[index];
        }

        private static void MultiplyP8(uint[] x)
        {
            uint num = x[3];
            GXDLMSCipheringStream.ShiftRight(x, 8);
            for (int index = 0; index != 8; ++index)
            {
                if (((ulong)num & (ulong)(1 << index)) > 0UL)
                    x[0] ^= 3774873600U >> 7 - index;
            }
        }

        private byte[] GetGHash(byte[] b)
        {
            byte[] block = new byte[16];
            for (int sourceIndex = 0; sourceIndex < b.Length; sourceIndex += 16)
            {
                byte[] destinationArray = new byte[16];
                int length = Math.Min(b.Length - sourceIndex, 16);
                Array.Copy((Array)b, sourceIndex, (Array)destinationArray, 0, length);
                GXDLMSCipheringStream.Xor(block, destinationArray);
                this.MultiplyH(block);
            }
            return block;
        }

        private static void UInt32_To_BE(uint value, byte[] buff, int offset)
        {
            buff[offset] = (byte)(value >> 24);
            buff[++offset] = (byte)(value >> 16);
            buff[++offset] = (byte)(value >> 8);
            buff[++offset] = (byte)value;
        }

        private void MultiplyH(byte[] value)
        {
            uint[] numArray1 = new uint[4];
            for (int index = 0; index != 16; ++index)
            {
                uint[] numArray2 = this.M[index + index][(int)value[index] & 15];
                numArray1[0] ^= numArray2[0];
                numArray1[1] ^= numArray2[1];
                numArray1[2] ^= numArray2[2];
                numArray1[3] ^= numArray2[3];
                uint[] numArray3 = this.M[index + index + 1][((int)value[index] & 240) >> 4];
                numArray1[0] ^= numArray3[0];
                numArray1[1] ^= numArray3[1];
                numArray1[2] ^= numArray3[2];
                numArray1[3] ^= numArray3[3];
            }
            GXDLMSCipheringStream.UInt32_To_BE(numArray1[0], value, 0);
            GXDLMSCipheringStream.UInt32_To_BE(numArray1[1], value, 4);
            GXDLMSCipheringStream.UInt32_To_BE(numArray1[2], value, 8);
            GXDLMSCipheringStream.UInt32_To_BE(numArray1[3], value, 12);
        }

        private void Init(byte[] H)
        {
            this.M[0] = new uint[16][];
            this.M[1] = new uint[16][];
            this.M[0][0] = new uint[4];
            this.M[1][0] = new uint[4];
            this.M[1][8] = GXDLMSCipheringStream.GetUint128(H);
            for (int index = 4; index >= 1; index >>= 1)
            {
                uint[] x = (uint[])this.M[1][index + index].Clone();
                GXDLMSCipheringStream.MultiplyP(x);
                this.M[1][index] = x;
            }
            uint[] x1 = (uint[])this.M[1][1].Clone();
            GXDLMSCipheringStream.MultiplyP(x1);
            this.M[0][8] = x1;
            for (int index = 4; index >= 1; index >>= 1)
            {
                uint[] x2 = (uint[])this.M[0][index + index].Clone();
                GXDLMSCipheringStream.MultiplyP(x2);
                this.M[0][index] = x2;
            }
            int index1 = 0;
            while (true)
            {
                for (int index2 = 2; index2 < 16; index2 += index2)
                {
                    for (int index3 = 1; index3 < index2; ++index3)
                    {
                        uint[] block = (uint[])this.M[index1][index2].Clone();
                        GXDLMSCipheringStream.Xor(block, this.M[index1][index3]);
                        this.M[index1][index2 + index3] = block;
                    }
                }
                if (++index1 != 32)
                {
                    if (index1 > 1)
                    {
                        this.M[index1] = new uint[16][];
                        this.M[index1][0] = new uint[4];
                        for (int index4 = 8; index4 > 0; index4 >>= 1)
                        {
                            uint[] x3 = (uint[])this.M[index1 - 2][index4].Clone();
                            GXDLMSCipheringStream.MultiplyP8(x3);
                            this.M[index1][index4] = x3;
                        }
                    }
                }
                else
                    break;
            }
        }

        private void gCTRBlock(byte[] buf, int bufCount)
        {
            int index1 = 15;
            while (index1 >= 12 && ++this.counter[index1] <= (byte)0)
                --index1;
            byte[] numArray1 = new byte[16];
            this.ProcessBlock(this.counter, 0, numArray1, 0);
            byte[] numArray2;
            if (this.Encrypt)
            {
                Array.Copy((Array)GXDLMSCipheringStream.Zeroes, bufCount, (Array)numArray1, bufCount, 16 - bufCount);
                numArray2 = numArray1;
            }
            else
                numArray2 = buf;
            for (int index2 = 0; index2 != bufCount; ++index2)
            {
                numArray1[index2] ^= buf[index2];
                this.Output.Add(numArray1[index2]);
            }
            GXDLMSCipheringStream.Xor(this.S, numArray2);
            this.MultiplyH(this.S);
            this.totalLength += (ulong)bufCount;
        }

        private static void SetPackLength(ulong length, byte[] buff, int offset)
        {
            GXDLMSCipheringStream.UInt32_To_BE((uint)(length >> 32), buff, offset);
            GXDLMSCipheringStream.UInt32_To_BE((uint)length, buff, offset + 4);
        }

        private void Reset()
        {
            this.S = this.GetGHash(this.Aad);
            this.counter = (byte[])this.J0.Clone();
            this.BytesRemaining = 0;
            this.totalLength = 0UL;
        }

        internal static bool TagsEquals(byte[] tag1, byte[] tag2)
        {
            for (int index = 0; index != 12; ++index)
            {
                if ((int)tag1[index] != (int)tag2[index])
                    return false;
            }
            return true;
        }

        public virtual void Write(byte[] input)
        {
            foreach (byte num in input)
            {
                this.bufBlock[this.BytesRemaining++] = num;
                if (this.BytesRemaining == 16)
                {
                    this.gCTRBlock(this.bufBlock, 16);
                    if (!this.Encrypt)
                        Array.Copy((Array)this.bufBlock, 16, (Array)this.bufBlock, 0, this.Tag.Length);
                    this.BytesRemaining = 0;
                }
            }
        }





        public byte[] FlushFinalBlock()
        {
            if (this.BytesRemaining > 0)
            {
                byte[] numArray = new byte[16];
                Array.Copy((Array)this.bufBlock, 0, (Array)numArray, 0, this.BytesRemaining);
                this.gCTRBlock(numArray, this.BytesRemaining);
            }
            if (this.Security == Security.Encryption)
            {
                this.Reset();
                return this.Output.ToArray();
            }
            byte[] buff = new byte[16];
            GXDLMSCipheringStream.SetPackLength((ulong)this.Aad.Length * 8UL, buff, 0);
            GXDLMSCipheringStream.SetPackLength(this.totalLength * 8UL, buff, 8);
            GXDLMSCipheringStream.Xor(this.S, buff);
            this.MultiplyH(this.S);
            byte[] numArray1 = new byte[16];
            this.ProcessBlock(this.J0, 0, numArray1, 0);
            GXDLMSCipheringStream.Xor(numArray1, this.S);
            if (!this.Encrypt)
            {
                if (!GXDLMSCipheringStream.TagsEquals(this.Tag, numArray1))
                    throw new GXDLMSException("Decrypt failed. Invalid tag.");
            }
            else
                Array.Copy((Array)numArray1, 0, (Array)this.Tag, 0, 12);
            this.Reset();
            return this.Output.ToArray();
        }

        private uint[,] GenerateKey(bool encrypt, byte[] key)
        {
            int num1 = key.Length / 4;
            this.Rounds = num1 + 6;
            uint[,] key1 = new uint[this.Rounds + 1, 4];
            int num2 = 0;
            int offset = 0;
            while (offset < key.Length)
            {
                key1[num2 >> 2, num2 & 3] = GXDLMSCipheringStream.ToUInt32(key, offset);
                offset += 4;
                ++num2;
            }
            int num3 = this.Rounds + 1 << 2;
            for (int index = num1; index < num3; ++index)
            {
                uint num4 = key1[index - 1 >> 2, index - 1 & 3];
                if (index % num1 == 0)
                    num4 = GXDLMSCipheringStream.SubWord(this.Shift(num4, 8)) ^ (uint)GXDLMSCipheringStream.rcon[index / num1 - 1];
                else if (num1 > 6 && index % num1 == 4)
                    num4 = GXDLMSCipheringStream.SubWord(num4);
                key1[index >> 2, index & 3] = key1[index - num1 >> 2, index - num1 & 3] ^ num4;
            }
            if (!encrypt)
            {
                for (int index1 = 1; index1 < this.Rounds; ++index1)
                {
                    for (int index2 = 0; index2 < 4; ++index2)
                        key1[index1, index2] = this.ImixCol(key1[index1, index2]);
                }
            }
            return key1;
        }
    }
}
