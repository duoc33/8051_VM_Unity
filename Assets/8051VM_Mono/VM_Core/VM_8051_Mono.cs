using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public delegate void ReadSFR_Handler(byte data);
public delegate void ReadSFR_DotLed_Handler(byte P3_data, byte P0_data);
public delegate void ReadSFR_DotLed_Bit_Handler(byte P3_data, int bit, byte P0_data);
public delegate byte WriteSFR_Handler();
public class VM_8051_Mono
{
    #region 单例和初始化
    private static VM_8051_Mono instance = new VM_8051_Mono();
    public static VM_8051_Mono Instance => instance;
    private VM_8051_Mono()
    {
        code = new byte[65536];
        external_ram = new byte[65536];
        sfr_ram = new byte[256];
        opcodeDic = new Dictionary<byte, string>();
        opcodeDic.Clear();
        instr = new _instruct();
        Init_VM();
    }
    private void Init_VM()
    {
        AddDic();
        // Debug.Log(opcodeDic.Count);
        Reset();
        vm_Interupt = new VM_Interupt_System();
    }
    public void Reset()
    {
        PC = 0x0000;
        cycles = 0;
        in_interupt = false;
        byte auxr = (byte)(sfr_ram[AUXR] & 0xFC);
        byte auxr1 = (byte)(sfr_ram[AUXR1] & 0xF6);
        byte sbuf = sfr_ram[SBUF];
        byte pcon = (byte)((sfr_ram[PCON] & 0x30) | 0x10);
        byte ie = (byte)(sfr_ram[IE] & 0x40);
        byte ip = (byte)(sfr_ram[IP] & 0xC0);
        byte t2mod = (byte)(sfr_ram[T2MOD] & 0xFC);
        byte wdt = (byte)(sfr_ram[WDT] & 0xC0);
        byte isp_cmd = (byte)(sfr_ram[ISP_CMD] & 0xF8);
        byte isp_trig = sfr_ram[ISP_TRIG];
        byte isp_contr = (byte)(sfr_ram[ISP_CONTR] & 0x18);
        byte p4 = (byte)(sfr_ram[P4] | 0x0f);

        Array.Clear(external_ram, 0, external_ram.Length);
        Array.Clear(sfr_ram, 0, sfr_ram.Length);
        instr = new _instruct();

        Write_Sfr(P0, 0xFF);
        Write_Sfr(P1, 0xFF);
        Write_Sfr(P2, 0xFF);
        Write_Sfr(P3, 0xFF);
        Write_Sfr(ISP_DATA, 0xFF);//ISP/IAP 数据寄存器
        Write_Sfr(AUXR, auxr);
        Write_Sfr(AUXR1, auxr1);
        Write_Sfr(SBUF, sbuf);
        Write_Sfr(PCON, pcon);
        Write_Sfr(IE, ie);
        Write_Sfr(IP, ip);
        Write_Sfr(T2MOD, t2mod);
        Write_Sfr(WDT, wdt);
        Write_Sfr(ISP_CMD, isp_cmd);
        Write_Sfr(ISP_TRIG, isp_trig);
        Write_Sfr(ISP_CONTR, isp_contr);
        Write_Sfr(SP, 0x7);
    }
    #endregion
    #region 机器码映射
    private void AddDic()
    {
        opcodeDic.Add(0x00, "NOP");
        opcodeDic.Add(0x80, "SJMP");

        AddBitAlgorithm();
        AddAlgorithm();
        AddLogic();
        AddXCH();
        ADDMove();
        AddPop_Push();
        AddJMP();
    }
    private void AddBitAlgorithm()
    {
        opcodeDic.Add(0x82, "ANL_C_BIT");
        opcodeDic.Add(0xB0, "ANL_C_NOT_BIT");
        opcodeDic.Add(0x72, "ORL_C_BIT");
        opcodeDic.Add(0xA0, "ORL_C_NOT_BIT");
        opcodeDic.Add(0xC3, "CLR_C");
        opcodeDic.Add(0xC2, "CLR_BIT");
        opcodeDic.Add(0xD3, "SETB_C");
        opcodeDic.Add(0xD2, "SETB_BIT");
        opcodeDic.Add(0xB3, "CPL_C");//取反
        opcodeDic.Add(0xB2, "CPL_BIT");
        opcodeDic.Add(0x40, "JC_rel");
        opcodeDic.Add(0x50, "JNC_rel");

        opcodeDic.Add(0x10, "JBC_BIT_rel");
        opcodeDic.Add(0x20, "JB_BIT_rel");
        opcodeDic.Add(0x30, "JNB_BIT_rel");

    }
    private void AddJMP()
    {
        //AJMP addr11
        opcodeDic.Add(0x01, "AJMP_ADDR");
        opcodeDic.Add(0x21, "AJMP_ADDR");
        opcodeDic.Add(0x41, "AJMP_ADDR");
        opcodeDic.Add(0x61, "AJMP_ADDR");
        opcodeDic.Add(0x81, "AJMP_ADDR");
        opcodeDic.Add(0xA1, "AJMP_ADDR");
        opcodeDic.Add(0xC1, "AJMP_ADDR");
        opcodeDic.Add(0xE1, "AJMP_ADDR");
        //LJMP addr16
        opcodeDic.Add(0x02, "LJMP_ADDR");
        //ACALL addr11
        opcodeDic.Add(0x11, "ACALL_ADDR");
        opcodeDic.Add(0x31, "ACALL_ADDR");
        opcodeDic.Add(0x51, "ACALL_ADDR");
        opcodeDic.Add(0x71, "ACALL_ADDR");
        opcodeDic.Add(0x91, "ACALL_ADDR");
        opcodeDic.Add(0xB1, "ACALL_ADDR");
        opcodeDic.Add(0xD1, "ACALL_ADDR");
        opcodeDic.Add(0xF1, "ACALL_ADDR");
        //LCALL addr16
        opcodeDic.Add(0x12, "LCALL_ADDR");
        //RET
        opcodeDic.Add(0x22, "RET");
        //RETI
        opcodeDic.Add(0x32, "RETI");
        //JMP @A+DPTR
        opcodeDic.Add(0x73, "JMP_AT_ACC_DPTR");
        //条件跳转
        opcodeDic.Add(0x60, "JZ_rel");
        opcodeDic.Add(0x70, "JNZ_rel");
        opcodeDic.Add(0xD8, "DJNZ_Rn_rel");
        opcodeDic.Add(0xD9, "DJNZ_Rn_rel");
        opcodeDic.Add(0xDA, "DJNZ_Rn_rel");
        opcodeDic.Add(0xDB, "DJNZ_Rn_rel");
        opcodeDic.Add(0xDC, "DJNZ_Rn_rel");
        opcodeDic.Add(0xDD, "DJNZ_Rn_rel");
        opcodeDic.Add(0xDE, "DJNZ_Rn_rel");
        opcodeDic.Add(0xDF, "DJNZ_Rn_rel");
        opcodeDic.Add(0xD5, "DJNZ_DIRECT_rel");

        opcodeDic.Add(0xB5, "CJNE_ACC_DIRECT_rel");
        opcodeDic.Add(0xB4, "CJNE_ACC_IMM_rel");
        opcodeDic.Add(0xB8, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xB9, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xBA, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xBB, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xBC, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xBD, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xBE, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xBF, "CJNE_Rn_IMM_rel");
        opcodeDic.Add(0xB6, "CJNE_AT_Ri_IMM_rel");
        opcodeDic.Add(0xB7, "CJNE_AT_Ri_IMM_rel");
    }
    private void AddAlgorithm()
    {
        //ADD
        opcodeDic.Add(0x24, "ADD_A_IMM");
        opcodeDic.Add(0x25, "ADD_A_DIRECT");
        opcodeDic.Add(0x26, "ADD_A_AT_Ri");
        opcodeDic.Add(0x27, "ADD_A_AT_Ri");
        opcodeDic.Add(0x28, "ADD_A_Rn");
        opcodeDic.Add(0x29, "ADD_A_Rn");
        opcodeDic.Add(0x2A, "ADD_A_Rn");
        opcodeDic.Add(0x2B, "ADD_A_Rn");
        opcodeDic.Add(0x2C, "ADD_A_Rn");
        opcodeDic.Add(0x2D, "ADD_A_Rn");
        opcodeDic.Add(0x2E, "ADD_A_Rn");
        opcodeDic.Add(0x2F, "ADD_A_Rn");
        //ADDC
        opcodeDic.Add(0x34, "ADDC_A_IMM");
        opcodeDic.Add(0x35, "ADDC_A_DIRECT");
        opcodeDic.Add(0x36, "ADDC_A_AT_Ri");
        opcodeDic.Add(0x37, "ADDC_A_AT_Ri");
        opcodeDic.Add(0x38, "ADDC_A_Rn");
        opcodeDic.Add(0x39, "ADDC_A_Rn");
        opcodeDic.Add(0x3A, "ADDC_A_Rn");
        opcodeDic.Add(0x3B, "ADDC_A_Rn");
        opcodeDic.Add(0x3C, "ADDC_A_Rn");
        opcodeDic.Add(0x3D, "ADDC_A_Rn");
        opcodeDic.Add(0x3E, "ADDC_A_Rn");
        opcodeDic.Add(0x3F, "ADDC_A_Rn");
        //INC
        opcodeDic.Add(0x04, "INC_A");
        opcodeDic.Add(0x05, "INC_DIRECT");
        opcodeDic.Add(0x06, "INC_AT_Ri");
        opcodeDic.Add(0x07, "INC_AT_Ri");
        opcodeDic.Add(0x08, "INC_Rn");
        opcodeDic.Add(0x09, "INC_Rn");
        opcodeDic.Add(0x0A, "INC_Rn");
        opcodeDic.Add(0x0B, "INC_Rn");
        opcodeDic.Add(0x0C, "INC_Rn");
        opcodeDic.Add(0x0D, "INC_Rn");
        opcodeDic.Add(0x0E, "INC_Rn");
        opcodeDic.Add(0x0F, "INC_Rn");
        opcodeDic.Add(0xA3, "INC_DPTR");
        //DEC
        opcodeDic.Add(0x14, "DEC_A");
        opcodeDic.Add(0x15, "DEC_DIRECT");
        opcodeDic.Add(0x16, "DEC_AT_Ri");
        opcodeDic.Add(0x17, "DEC_AT_Ri");
        opcodeDic.Add(0x18, "DEC_Rn");
        opcodeDic.Add(0x19, "DEC_Rn");
        opcodeDic.Add(0x1A, "DEC_Rn");
        opcodeDic.Add(0x1B, "DEC_Rn");
        opcodeDic.Add(0x1C, "DEC_Rn");
        opcodeDic.Add(0x1D, "DEC_Rn");
        opcodeDic.Add(0x1E, "DEC_Rn");
        opcodeDic.Add(0x1F, "DEC_Rn");
        //SUBB
        opcodeDic.Add(0x94, "SUBB_A_IMM");
        opcodeDic.Add(0x95, "SUBB_A_DIRECT");
        opcodeDic.Add(0x96, "SUBB_A_AT_Ri");
        opcodeDic.Add(0x97, "SUBB_A_AT_Ri");
        opcodeDic.Add(0x98, "SUBB_A_Rn");
        opcodeDic.Add(0x99, "SUBB_A_Rn");
        opcodeDic.Add(0x9A, "SUBB_A_Rn");
        opcodeDic.Add(0x9B, "SUBB_A_Rn");
        opcodeDic.Add(0x9C, "SUBB_A_Rn");
        opcodeDic.Add(0x9D, "SUBB_A_Rn");
        opcodeDic.Add(0x9E, "SUBB_A_Rn");
        opcodeDic.Add(0x9F, "SUBB_A_Rn");
        //MUL
        opcodeDic.Add(0xA4, "MUL_AB");
        opcodeDic.Add(0x84, "DIV_AB");
        opcodeDic.Add(0xD4, "DA_A");



    }
    private void AddLogic()
    {
        opcodeDic.Add(0x58, "ANL_ACC_Rn");
        opcodeDic.Add(0x59, "ANL_ACC_Rn");
        opcodeDic.Add(0x5A, "ANL_ACC_Rn");
        opcodeDic.Add(0x5B, "ANL_ACC_Rn");
        opcodeDic.Add(0x5C, "ANL_ACC_Rn");
        opcodeDic.Add(0x5D, "ANL_ACC_Rn");
        opcodeDic.Add(0x5E, "ANL_ACC_Rn");
        opcodeDic.Add(0x5F, "ANL_ACC_Rn");
        opcodeDic.Add(0x55, "ANL_ACC_DIRECT");
        opcodeDic.Add(0x56, "ANL_ACC_AT_Ri");
        opcodeDic.Add(0x57, "ANL_ACC_AT_Ri");
        opcodeDic.Add(0x54, "ANL_ACC_IMM");
        opcodeDic.Add(0x52, "ANL_DIRECT_ACC");
        opcodeDic.Add(0x53, "ANL_DIRECT_IMM");

        opcodeDic.Add(0x48, "ORL_ACC_Rn");
        opcodeDic.Add(0x49, "ORL_ACC_Rn");
        opcodeDic.Add(0x4A, "ORL_ACC_Rn");
        opcodeDic.Add(0x4B, "ORL_ACC_Rn");
        opcodeDic.Add(0x4C, "ORL_ACC_Rn");
        opcodeDic.Add(0x4D, "ORL_ACC_Rn");
        opcodeDic.Add(0x4E, "ORL_ACC_Rn");
        opcodeDic.Add(0x4F, "ORL_ACC_Rn");
        opcodeDic.Add(0x45, "ORL_ACC_DIRECT");
        opcodeDic.Add(0x46, "ORL_ACC_AT_Ri");
        opcodeDic.Add(0x47, "ORL_ACC_AT_Ri");
        opcodeDic.Add(0x44, "ORL_ACC_IMM");
        opcodeDic.Add(0x42, "ORL_DIRECT_ACC");
        opcodeDic.Add(0x43, "ORL_DIRECT_IMM");

        opcodeDic.Add(0x68, "XRL_ACC_Rn");
        opcodeDic.Add(0x69, "XRL_ACC_Rn");
        opcodeDic.Add(0x6A, "XRL_ACC_Rn");
        opcodeDic.Add(0x6B, "XRL_ACC_Rn");
        opcodeDic.Add(0x6C, "XRL_ACC_Rn");
        opcodeDic.Add(0x6D, "XRL_ACC_Rn");
        opcodeDic.Add(0x6E, "XRL_ACC_Rn");
        opcodeDic.Add(0x6F, "XRL_ACC_Rn");
        opcodeDic.Add(0x65, "XRL_ACC_DIRECT");
        opcodeDic.Add(0x66, "XRL_ACC_AT_Ri");
        opcodeDic.Add(0x67, "XRL_ACC_AT_Ri");
        opcodeDic.Add(0x64, "XRL_ACC_IMM");
        opcodeDic.Add(0x62, "XRL_DIRECT_ACC");
        opcodeDic.Add(0x63, "XRL_DIRECT_IMM");

        opcodeDic.Add(0xE4, "CLR_A");
        opcodeDic.Add(0xF4, "CPL_A");

        opcodeDic.Add(0x23, "RL_A");
        opcodeDic.Add(0x33, "RLC_A");

        opcodeDic.Add(0x03, "RR_A");
        opcodeDic.Add(0x13, "RRC_A");


    }
    private void AddXCH()
    {
        opcodeDic.Add(0xC4, "SWAP_A");

        opcodeDic.Add(0xC5, "XCH_A_DIRECT");
        opcodeDic.Add(0xC6, "XCH_A_AT_Ri");
        opcodeDic.Add(0xC7, "XCH_A_AT_Ri");

        opcodeDic.Add(0xC8, "XCH_A_Rn");
        opcodeDic.Add(0xC9, "XCH_A_Rn");
        opcodeDic.Add(0xCA, "XCH_A_Rn");
        opcodeDic.Add(0xCB, "XCH_A_Rn");
        opcodeDic.Add(0xCC, "XCH_A_Rn");
        opcodeDic.Add(0xCD, "XCH_A_Rn");
        opcodeDic.Add(0xCE, "XCH_A_Rn");
        opcodeDic.Add(0xCF, "XCH_A_Rn");

        opcodeDic.Add(0xD6, "XCHD_A_AT_Ri");
        opcodeDic.Add(0xD7, "XCHD_A_AT_Ri");
    }
    private void AddPop_Push()
    {
        opcodeDic.Add(0xC0, "PUSH_DIRECT");
        opcodeDic.Add(0xD0, "POP_DIRECT");
    }
    private void ADDMove()
    {
        opcodeDic.Add(0x74, "MOVE_ACC_IMM");
        opcodeDic.Add(0x75, "MOVE_DIRECT_IMM");
        opcodeDic.Add(0x76, "MOVE_At_Ri_IMM");
        opcodeDic.Add(0x77, "MOVE_At_Ri_IMM");
        opcodeDic.Add(0x78, "MOVE_Rn_IMM");
        opcodeDic.Add(0x79, "MOVE_Rn_IMM");
        opcodeDic.Add(0x7A, "MOVE_Rn_IMM");
        opcodeDic.Add(0x7B, "MOVE_Rn_IMM");
        opcodeDic.Add(0x7C, "MOVE_Rn_IMM");
        opcodeDic.Add(0x7D, "MOVE_Rn_IMM");
        opcodeDic.Add(0x7E, "MOVE_Rn_IMM");
        opcodeDic.Add(0x7F, "MOVE_Rn_IMM");
        opcodeDic.Add(0x83, "MOVC_ACC_At_ACC_PC");
        opcodeDic.Add(0x85, "MOVE_DIRECT_DIRECT");//这个指令 des 和 src是颠倒的
        opcodeDic.Add(0x86, "MOVE_DIRECT_At_Rn");
        opcodeDic.Add(0x87, "MOVE_DIRECT_At_Rn");
        opcodeDic.Add(0x88, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x89, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x8A, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x8B, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x8C, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x8D, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x8E, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x8F, "MOVE_DIRECT_Rn");
        opcodeDic.Add(0x90, "MOVE_DPTR_IMM16");
        opcodeDic.Add(0x92, "MOVE_BIT_Cy");
        opcodeDic.Add(0x93, "MOVC_ACC_At_ACC_DPTR");
        opcodeDic.Add(0xA2, "MOVE_Cy_BIT");
        opcodeDic.Add(0xA6, "MOVE_At_Ri_DIRECT");
        opcodeDic.Add(0xA7, "MOVE_At_Ri_DIRECT");
        opcodeDic.Add(0xA8, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xA9, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xAA, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xAB, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xAC, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xAD, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xAE, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xAF, "MOVE_Rn_DIRECT");
        opcodeDic.Add(0xE0, "MOVX_ACC_AT_DPTR");
        opcodeDic.Add(0xE2, "MOVX_ACC_AT_Ri");
        opcodeDic.Add(0xE3, "MOVX_ACC_AT_Ri");
        opcodeDic.Add(0xE5, "MOVE_ACC_DIRECT");
        opcodeDic.Add(0xE6, "MOVE_ACC_At_Ri");//间接寻址 R0,R1
        opcodeDic.Add(0xE7, "MOVE_ACC_At_Ri");
        opcodeDic.Add(0xE8, "MOVE_ACC_Rn");
        opcodeDic.Add(0xE9, "MOVE_ACC_Rn");
        opcodeDic.Add(0xEA, "MOVE_ACC_Rn");
        opcodeDic.Add(0xEB, "MOVE_ACC_Rn");
        opcodeDic.Add(0xEC, "MOVE_ACC_Rn");
        opcodeDic.Add(0xED, "MOVE_ACC_Rn");
        opcodeDic.Add(0xEE, "MOVE_ACC_Rn");
        opcodeDic.Add(0xEF, "MOVE_ACC_Rn");

        opcodeDic.Add(0xF0, "MOVX_AT_DPTR_ACC");
        opcodeDic.Add(0xF2, "MOVX_AT_Ri_ACC");
        opcodeDic.Add(0xF3, "MOVX_AT_Ri_ACC");

        opcodeDic.Add(0xF5, "MOVE_DIRECT_ACC");
        opcodeDic.Add(0xF6, "MOVE_At_Ri_ACC");
        opcodeDic.Add(0xF7, "MOVE_At_Ri_ACC");
        opcodeDic.Add(0xF8, "MOVE_Rn_ACC");
        opcodeDic.Add(0xF9, "MOVE_Rn_ACC");
        opcodeDic.Add(0xFA, "MOVE_Rn_ACC");
        opcodeDic.Add(0xFB, "MOVE_Rn_ACC");
        opcodeDic.Add(0xFC, "MOVE_Rn_ACC");
        opcodeDic.Add(0xFD, "MOVE_Rn_ACC");
        opcodeDic.Add(0xFE, "MOVE_Rn_ACC");
        opcodeDic.Add(0xFF, "MOVE_Rn_ACC");
    }
    #endregion
 
    #region 回调 外部获取数据的接口
    //Led数据
    public ReadSFR_Handler read_P2_data_handler;
    //DotLed数据
    public ReadSFR_DotLed_Handler read_P3_456_data_handler;
    public ReadSFR_DotLed_Bit_Handler read_P3_456_data_bit_handler;
    //独立按键输入的数据接口 k3 k4作为外部中断的处理
    public WriteSFR_Handler has_Input_Alone_handler;
    public bool isInputAloneKey = false;
    public bool isInputK3AloneKey = false;
    public bool isInputK4AloneKey = false;
    byte nowP3_Kn_data;
    byte inputP3_Kn_Data;

    //矩阵按键的输入值
    byte nowP1data; //当前P1被输出的值
    byte inputP1Data;//当前输入的按键值
    public bool isInput = false;//矩阵按键是否输入
    public WriteSFR_Handler has_Input_P1_data_handler;
    //Uart接收发送
    public bool isInputChar = false;
    byte InputTextData;
    public ReadSFR_Handler Sbuf_Transmit_handler;//Uart发送
    public WriteSFR_Handler Sbuf_Receive_handler;//Uart接收


    //外部调用接口,读写SFR
    public void WriteSFR_BIT_ACTION(byte addr,int bit) 
    {
        Write_Bit_Default(addr, bit);
    }
    public byte ReadSFR_BIT_ACTION(byte addr)
    {
       return Read_Bit_Default(addr);
    }
    public void WriteSFR_ACTION(byte addr,byte data) 
    {
        sfr_ram[addr] = data;
    }
    public byte ReadSFR_Action(byte addr)
    {
        byte data = sfr_ram[addr];
        return data;
    }

    #endregion

    #region 数据结构 
    public bool isRunning = false;
    private Dictionary<byte, string> opcodeDic;
    public _instruct instr;
    private VM_Interupt_System vm_Interupt;
    public bool in_interupt = false;
    public ushort PC;
    public int cycles;
    private byte[] code;
    private byte[] sfr_ram;
    private byte[] external_ram;

    private readonly byte A = 0xE0;
    private readonly byte B = 0xF0;
    private readonly byte SP = 0x81;
    private readonly byte DPTR_L = 0x82;
    private readonly byte DPTR_H = 0x83;
    //程序状态字特殊功能寄存器
    private readonly byte PSW = 0xD0;
    private readonly byte PSW_P = 0xD0 + 0; //ACC里1的个数为奇数时置1
    //private readonly byte PSW_OV = 0xD0+2;
    //private readonly byte PSW_RS0 = 0xD0+3;
    //private readonly byte PSW_RS1 = 0xD0+4;
    //private readonly byte PSW_F0 = 0xD0+5;
    //private readonly byte PSW_AC = 0xD0+6;
    private readonly byte PSW_CY = 0xD0 + 7;
    //定时器/计数器 工作模式寄存器
    //辅助寄存器 xxxx xx00
    private readonly byte AUXR = 0x8E;
    //辅助寄存器1
    private readonly byte AUXR1 = 0xA2;
    //PCON 电源控制寄存器
    private readonly byte PCON = 0x87;
    #region 中断 相关
    //中断允许寄存器
    private readonly byte IE = 0xA8;
    //中断优先级寄存器
    private readonly byte IP = 0xB8;
    //中断优先高
    private readonly byte IPH = 0xB7;
    //串行数据缓冲特殊功能寄存器
    //private const byte SBUF = 0x99;
    //定时器计数器2
    private readonly byte T2CON = 0xC8;
    //辅助中断控制器
    private readonly byte XICON = 0xC0;
    #endregion
    //定时器2模式模式寄存器
    private readonly byte T2MOD = 0xC9;
    //看门狗控制寄存器
    private readonly byte WDT = 0xE1;
    //ISP/IAP数据寄存器
    private readonly byte ISP_DATA = 0xE2;
    // ISP/IAP命令寄存器
    private readonly byte ISP_CMD = 0xE5;
    //ISP/IAP命令触发寄存器
    private readonly byte ISP_TRIG = 0xE6;
    //ISP/IAP控制寄存器
    private readonly byte ISP_CONTR = 0xE7;
    //P4口
    private readonly byte P4 = 0xE8;
    // I/O口
    private const byte P0 = 0x80;
    private const byte P1 = 0x90;
    private const byte P2 = 0xA0;
    private const byte P3 = 0xB0;
    //默认放在0区，具体是由PSW中RS1 RS0两个位选决定的
    private readonly byte R0 = 0x00;
    private readonly byte R1 = 0x01;
    private readonly byte R2 = 0x02;
    private readonly byte R3 = 0x03;
    private readonly byte R4 = 0x04;
    private readonly byte R5 = 0x05;
    private readonly byte R6 = 0x06;
    private readonly byte R7 = 0x07;
    private readonly byte sfr_addr_start = 0x80;
    private readonly byte ram_bit_addr_start = 0x20;
    #endregion

    #region 打印工具
    public int vm_cycles() => cycles;
    public ushort vm_pc() => PC;
    private void PrintRegResult()
    {
        Debug.Log("RO: " + VmRead(mem_type.RAM, (ushort)(R0 +get_psw_rs()*8)).ToString("x"));
        Debug.Log("R1: " + VmRead(mem_type.RAM, (ushort)(R1 + get_psw_rs() * 8)).ToString("x"));
        Debug.Log("R2: " + VmRead(mem_type.RAM, (ushort)(R2 + get_psw_rs() * 8)).ToString("x"));
        Debug.Log("R3: " + VmRead(mem_type.RAM, (ushort)(R3 + get_psw_rs() * 8)).ToString("x"));
        Debug.Log("R4: " + VmRead(mem_type.RAM, (ushort)(R4 + get_psw_rs() * 8)).ToString("x"));
        Debug.Log("R5: " + VmRead(mem_type.RAM, (ushort)(R5 + get_psw_rs() * 8)).ToString("x"));
        Debug.Log("R6: " + VmRead(mem_type.RAM, (ushort)(R6 + get_psw_rs() * 8)).ToString("x"));
        Debug.Log("R7: " + VmRead(mem_type.RAM, (ushort)(R7 + get_psw_rs() * 8)).ToString("x"));
        Debug.Log("ACC: " + VmRead(mem_type.SFR, A).ToString("x"));
        Debug.Log("B: " + VmRead(mem_type.SFR, B).ToString("x"));
        Debug.Log("SP: " + VmRead(mem_type.SFR, SP).ToString("x"));
        Debug.Log("DPTR: " + (VmRead(mem_type.SFR, DPTR_H) << 8 | VmRead(mem_type.SFR, DPTR_L)).ToString("x"));
        Debug.Log("PC: " + vm_pc().ToString("x"));
        Debug.Log("Cycles: " + vm_cycles());
        Debug.Log("PSW: " + VmRead(mem_type.SFR, PSW).ToString("x"));
    }
    private void Show_Disa()
    {
        InstructInfo info = VM_8051_Mono.Instance.instr.info;
        if (VM_8051_Mono.Instance.opcodeDic.ContainsKey(VM_8051_Mono.Instance.instr.opcode))
        {
            string opcodeHex = VM_8051_Mono.Instance.instr.opcode.ToString("X");
            string op0 = VM_8051_Mono.Instance.instr.op0.ToString("X");
            string op1 = VM_8051_Mono.Instance.instr.op1.ToString("X");
            if (info.bytes == 1)
            {
                Debug.Log($"opcode 0x{opcodeHex} : ASM_Name {info.opcode_name} ");
            }
            else if (info.bytes == 2)
            {
                Debug.Log($"opcode 0x{opcodeHex}: ASM_Name {info.opcode_name} , opNum0 0x{op0} ");
            }
            else
            {
                Debug.Log($"opcode 0x{opcodeHex}: ASM_Name {info.opcode_name} ,opNum0 0x{op0} , opNum1 0x{op1}");
            }
            Debug.Log($"PC: {VM_8051_Mono.Instance.vm_pc().ToString("x")},Cycles: {VM_8051_Mono.Instance.vm_cycles()}");
        }
    }
    
    #endregion

    #region  加载、执行处理

    public void LoadProgram(byte[] program)
    {
        code = program;
    }
    private void FetchCode()
    {
        //取指令
        instr.opcode = code[PC];
        if (!opcodeDic.ContainsKey(instr.opcode)) { Debug.Log("没有该操作码"); return; }
        //动态实例指令信息类
        Type opcodeType = Type.GetType(opcodeDic[instr.opcode]);
        InstructInfo info = Activator.CreateInstance(opcodeType) as InstructInfo;
        instr.info = info;
        //赋值操作数
        instr.op0 = (info.bytes > 1) ? code[vm_pc() + 1] : (byte)0;
        instr.op1 = (info.bytes > 2) ? code[vm_pc() + 2] : (byte)0;
    }
    private void ExecuteInstruction()
    {
        if (!opcodeDic.ContainsKey(instr.opcode)) { Debug.Log("没有该操作码，不执行"); return; }
        instr.info.exec(instr);
        Update_PSW_P();
        Updata_timer(instr.info.cycles);
        if(isInputChar)
        {
            Write_Bit(SFR_SCON_RI, 1);
            isInputChar = false;
        }
        vm_Interupt.Execute_Interupt();
    }
    private const byte SBUF = 0x99;
    //串行控制特殊功能寄存器
    private const byte SCON = 0x98;
    private const byte SFR_SCON_REN = 0x98 + 4;
    private const byte SFR_SCON_SM1 = 0x98 + 4;//只模拟了SM1模式
    private const byte SFR_SCON_TI = 0x98 + 1;
    private const byte SFR_SCON_RI = 0x98 + 0;

    private readonly byte TMOD = 0x89;
    //定时器0的高八位
    private readonly byte TH0 = 0x8C;//非可位寻址寄存器
    //定时器0的低八位
    private readonly byte TL0 = 0x8A;
    //TCON 定时器/计数器 控制寄存器
    // private readonly byte TCON = 0x88;
    private readonly byte TCON_TR0 = 0x88 + 4;
    private readonly byte TCON_TF0 = 0x88 + 5;
    byte tmod;
    byte tr0;//计数器定时器开启标志位
    int count = 0;//计数
    private void Updata_timer(int Cycles) 
    {
        //读取定时器0的TR0位
        tr0=Read_Bit(TCON_TR0);
        //定时器启动，TR0置1才启动定时器
        if (tr0 != 0) 
        {
            //读取定时器工作模式
            tmod = Read_Sfr(TMOD);
            switch (tmod&0x3)
            {
                case 0x00:
                    break;
                //定时器16位的工作模式
                case 0x01:
                    {
                        count = (Read_Sfr(TH0) << 8) | Read_Sfr(TL0);
                        count += Cycles;
                        if (count > 0xffff)
                        {
                            Write_Bit(TCON_TF0, 1);
                            count = 0;
                        }
                        Write_Sfr(TH0, (byte)(count >> 8));
                        Write_Sfr(TL0, (byte)(count & 0xFF));
                        break;
                    }
                case 0x02:
                    break;
                case 0x03:
                    break;
                default:
                    break;
            }
        }
    }
    public void Run()
    {
        do
        {
            //Write_Sfr(P1, 0x77);
            FetchCode();
            ExecuteInstruction();
            //Show_Disa();
        } while (isRunning);
    }

    //A累加器中的值是否为奇数
    private void Update_PSW_P()
    {
        byte a = Read_Sfr(A);
        int count = 0;
        bool handler = true;
        while (handler)
        {
            if ((a & 0x1) != 0x0)
            {
                count++;
            }
            a >>= 1;
            if (a == 0x0)
            {
                handler = false;
            }
        }
        Write_Bit(PSW_P, count & 0x1);//奇数第一位一定是1,PSW_P 置为1
    }
    #endregion

    #region 读写内存 寻址 程序等
    private int get_psw_rs()
    {
        return (Read_Sfr(PSW)>>3)&0x3;
    }
    public ushort Read_Op(_instruct instruct, int opPosition, mem_type type)
    {
        //读取操作数的数据(可能时在寄存器中，可能是立即数，可能是直接寻址)
        InstructInfo info = instruct.info;
        //找到要读取的op的类型，(立即数，直接地址，寄存器地址，。。。) opPosition=1,就读1
        OpType op_mode = opPosition > 0 ? info.op1_mode : info.op0_mode;
        byte op = getOp(instruct, opPosition);
        switch (op_mode)
        {
            case OpType.NONE:
                return 0;
            case OpType.Rn:
                {
                    //0000 0111 = 0x07
                    int reg = instruct.opcode & 0x07;
                    return sfr_ram[reg+get_psw_rs()*8];
                }
            case OpType.IMM8:
                return op;
            case OpType.ACC:
                return Read_Sfr(A);
            case OpType.DIRECT:
                return Read_Ram(op);
            case OpType.DPTR:
                return (ushort)((Read_Sfr(DPTR_H) << 8) + Read_Sfr(DPTR_L));
            case OpType.IMM16:
                return (ushort)((instruct.op0 << 8) | instruct.op1);
            case OpType.R0_R1:
                byte addr = sfr_ram[instruct.opcode & 0x01+get_psw_rs() * 8];//找到Ri中存的地址，再寻址其中的数据
                return Read_Ram(addr);
            case OpType.PSW_Cy:
                return Read_Bit(PSW_CY);
            case OpType.BIT:
                return Read_Bit(op);
            case OpType.ACC_DPTR:
                ushort dptr = (ushort)((Read_Sfr(DPTR_H) << 8) | Read_Sfr(DPTR_L));
                ushort addr_A_D = (ushort)(Read_Sfr(A) + dptr);
                return VmRead(type, addr_A_D);
            case OpType.ACC_PC://对PC有影响，base.exec(instr)  要放之后
                ushort addr_A_P = (ushort)(Read_Sfr(A) + (PC + 1));
                return VmRead(type, addr_A_P);
            case OpType.AT_DPTR:
                ushort addr_AT_D = (ushort)((Read_Sfr(DPTR_H) << 8) | Read_Sfr(DPTR_L));
                return VmRead(type, addr_AT_D);
            case OpType.AT_SP:
                ushort addr_dir = Read_Sfr(SP);
                return VmRead(type, (byte)addr_dir);
            case OpType.B:
                return Read_Sfr(B);
            default:
                return 0;
        }
    }
    public void Write_Op(_instruct instruct, ushort data, int opPosition, mem_type type)
    {
        InstructInfo info = instruct.info;
        OpType op_mode = opPosition > 0 ? info.op1_mode : info.op0_mode;//操作数寻址模式
        byte op = getOp(instruct, opPosition);
        //IMM8 IMM16 不会写立即数
        switch (op_mode)
        {
            case OpType.NONE:
                break;
            case OpType.Rn:
                int reg = instruct.opcode & 0x07;
                sfr_ram[reg + get_psw_rs() * 8] = (byte)data;
                break;
            case OpType.ACC:
                Write_Sfr(A, (byte)data);
                break;
            case OpType.DIRECT:
                Write_Ram(op, (byte)data);
                break;
            case OpType.DPTR:
                Write_Sfr(DPTR_H, (byte)(data >> 8));
                Write_Sfr(DPTR_L, (byte)data);
                break;
            case OpType.R0_R1:
                byte addr = sfr_ram[instruct.opcode & 0x1+get_psw_rs() * 8];
                Write_Ram(addr, (byte)data);
                break;
            case OpType.PSW_Cy:
                Write_Bit(PSW_CY, data);
                break;
            case OpType.BIT:
                Write_Bit(op, (byte)data);
                break;
            case OpType.AT_DPTR:
                ushort dptr = (ushort)((Read_Sfr(DPTR_H) << 8) + Read_Sfr(DPTR_L));
                VmWrite(type, dptr, (byte)data);
                break;
            case OpType.AT_SP:
                ushort addr_dir = Read_Sfr(SP);
                VmWrite(type, (byte)addr_dir, (byte)data);
                break;
            case OpType.B:
                Write_Sfr(B, (byte)data);
                break;
            default:
                break;
        }
    }
    public byte VmRead(mem_type type, ushort addr)
    {
        switch (type)
        {
            case mem_type.CODE:
                return Read_Code(addr);
            case mem_type.RAM:
                return Read_Ram((byte)addr);
            case mem_type.SFR:
                return Read_Sfr((byte)addr);
            case mem_type.BIT:
                return Read_Bit((byte)addr);
            case mem_type.Exteranl:
                return Read_External(addr);
            default:
                return 0;
        }
    }
    public void VmWrite(mem_type type, ushort addr, byte data)
    {
        switch (type)
        {
            case mem_type.CODE:
                Write_Code(addr, data);
                break;
            case mem_type.RAM:
                Write_Ram((byte)addr, data);
                break;
            case mem_type.SFR:
                Write_Sfr((byte)addr, data);
                break;
            case mem_type.BIT:
                Write_Bit((byte)addr, data);
                break;
            case mem_type.Exteranl:
                Write_External(addr, data);
                break;
            default:
                break;
        }
    }

    //从指令中获得相应类型的字节 不是所有类型都需要从这里获取数据
    private byte getOp(_instruct instruct, int opPosition)
    {
        InstructInfo info = instruct.info;
        if (opPosition == 0)
        {
            switch (info.op0_mode)
            {
                case OpType.NONE:
                    return 0;
                case OpType.IMM8:
                case OpType.DIRECT:
                case OpType.BIT:
                    return instruct.op0;//这个也可以是 RAM中的地址(直接地址)
                default:
                    return 0;
            }
        }
        else
        {
            return info.bytes > 2 ? instruct.op1 : instruct.op0;
        }
    }
    public byte Read_Ram(byte addr)
    {
        byte data;
        if (addr >= 128)
            data = Read_Sfr(addr);
        else
            data = sfr_ram[addr];
        return data;
    }
    public void Write_Ram(byte addr, byte data)
    {
        if (addr >= 128)
        {
            Write_Sfr(addr, data);
        }
        else
        {
            sfr_ram[addr] = data;
        }
    }
    private byte Read_Code(ushort addr)
    {
        return code[addr];
    }
    private void Write_Code(ushort addr, byte data)
    {
        code[addr] = data;
    }
    private byte Read_External(ushort addr)
    {
        return external_ram[addr];
    }
    private void Write_External(ushort addr, byte data)
    {
        external_ram[addr] = data;
    }

    public void Write_Sfr(byte addr, byte data)
    {
        switch (addr)
        {
            case SBUF:
                Sbuf_Transmit_handler?.Invoke(data);
                Write_Bit(SFR_SCON_TI,1);
                sfr_ram[addr] = data;
                break;
            case P3:
                sfr_ram[addr] = data;
                read_P3_456_data_handler?.Invoke(data,sfr_ram[P0]);
                break;
            //P2
            case P2:
                sfr_ram[addr] = data;
                read_P2_data_handler?.Invoke(data);
                break;
            default:
                {
                    if (addr >= sfr_addr_start)
                    {
                        sfr_ram[addr] = data;
                    }
                }
                break;
        }
    }

    public byte Read_Sfr(byte addr)
    {
        switch (addr)
        {
            case SBUF:
                InputTextData = (byte)(Sbuf_Receive_handler?.Invoke());
                if (InputTextData !=0) 
                {
                    sfr_ram[SBUF] = InputTextData;
                    return sfr_ram[SBUF];
                }
                InputTextData = (byte)0;
                return sfr_ram[SBUF];
            case P1:
                if (isInput)
                {
                    nowP1data = sfr_ram[P1];//0000 1111
                    inputP1Data = (byte)(has_Input_P1_data_handler?.Invoke()); //0111 0111 1000 1000
                    return (byte)(nowP1data & inputP1Data);
                }
                else
                {
                    //弱上拉，不用进行改变
                    return sfr_ram[P1];
                }
            default:
                return (addr >= sfr_addr_start) ? sfr_ram[addr] : (byte)0;
        }
    }

    private void Write_Bit(byte addr, int bit)
    {
        Write_Bit_Default(addr, bit);
        byte byte_idx = (byte)(addr / 8 * 8);
        switch (byte_idx)
        {
            //P2
            case P2:
                  read_P2_data_handler?.Invoke(sfr_ram[byte_idx]);
                break;
            default:
                break;
        }
    }
    private void Write_Bit_Default(byte addr, int bit)
    {
        byte bit_offset = (byte)(addr % 8);
        bit &= 0x1;//限制只取最低位
        if (addr < sfr_addr_start)
        {
            byte byte_idx = (byte)(addr / 8 + ram_bit_addr_start);
            //只是把sfr_ram[byte_idx]里面的特定位置0了，其他位置&操作后依然是原样。
            sfr_ram[byte_idx] &= (byte)~(1 << bit_offset);//把里面相应的位置，置0，其他是 1111 0111 这是偏移3个的效果
                                                          // | 或操作后 给特定位 赋值
            sfr_ram[byte_idx] |= (bit | 0x0) != 0x0 ? (byte)(1 << bit_offset) : (byte)0;
        }
        else
        {
            byte byte_idx = (byte)(addr / 8 * 8);
            read_P3_456_data_bit_handler?.Invoke(addr, bit,sfr_ram[P0]);
            sfr_ram[byte_idx] &= (byte)~(1 << bit_offset);
            sfr_ram[byte_idx] |= (bit | 0x0) != 0x0 ? (byte)(1 << bit_offset) : (byte)0;
        }
    }

    //位寻址 是 0x20~0x2f编得的128个位的地址，下面的addr是与其他不同的
    private byte Read_Bit(byte addr)
    {
        return Read_Bit_Default(addr);
    }
    private byte Read_Bit_Default(byte addr)
    {
        byte bit_offset = (byte)(addr % 8);//得到偏移量
        if (addr < sfr_addr_start)
        {
            byte byte_idx = (byte)(addr / 8 + ram_bit_addr_start);// addr/8 是 0x20开始 增长了多少字节
                                                                  //sfr_ram[byte_idx] 根据遍的虚拟地址得到的真实ram地址
                                                                  //真实地址里的 数值与0000 0001 << bit_idx 的偏移量 相与,如果不等于0，则说明该位是真的
            return (sfr_ram[byte_idx] & (1 << bit_offset)) != 0 ? (byte)1 : (byte)0;
        }
        else
        {
            //位偏移与上面的一样，addr%8 就是8除不尽的位偏移
            byte byte_idx = (byte)(addr / 8 * 8);//addr = 0x85  除以8 正好得到字节数(除不尽的会抹除)，
                                                 //再乘以一个字节的位数，正好等于当前寄存器地址 
            switch (byte_idx)
            {
                case P1:
                    if (isInput) 
                    {
                        nowP1data = sfr_ram[P1];
                        inputP1Data = (byte)(has_Input_P1_data_handler?.Invoke());
                        return (byte)((nowP1data & inputP1Data) & (1 << bit_offset)) != 0 ? (byte)1 : (byte)0;
                    }
                    else
                    {
                        return (sfr_ram[P1] & (1 << bit_offset)) != 0 ? (byte)1 : (byte)0;
                    }
                case P3:
                    if (isInputAloneKey) 
                    {
                        nowP3_Kn_data = sfr_ram[P3];
                        inputP3_Kn_Data = (byte)(has_Input_Alone_handler?.Invoke());
                        isInputK3AloneKey = inputP3_Kn_Data - P3 == 2 ? true : false;
                        isInputK4AloneKey = inputP3_Kn_Data - P3 == 3 ? true : false;
                        nowP3_Kn_data = (byte)(nowP3_Kn_data &( 0 << (inputP3_Kn_Data - P3)));
                        return (byte)(nowP3_Kn_data & (1 << bit_offset)) != 0 ? (byte)1 : (byte)0;
                    }
                    else
                    {
                        return (sfr_ram[byte_idx] & (1 << bit_offset)) != 0 ? (byte)1 : (byte)0;
                    }
                default:
                    return (sfr_ram[byte_idx] & (1 << bit_offset)) != 0 ? (byte)1 : (byte)0;
            }
        }
    }
    #endregion
}