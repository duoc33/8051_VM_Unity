

/// <summary>
//● 立即寻址：操作数以指令字节的形式放在程序存储器中，为8位或16位。例如:MOV A, #30H、MOV DPTR, #8000H
//● 寄存器寻址：操作数放在选定的寄存器中，如R0-R7、A、B、DPTR，例如: MOV A, R3
//● 直接寻址：在指令中给定了操作数的实际地址。例如：MOV A, 30H，ANL 40H, #33H
//● 寄存器间接寻址：操作数的地址放在选定的寄存器中，如@R0-R7、SP、@DPTR、例如: MOV @R0, A
//● 基址+变址间接寻址：以DPTR或PC作基址寄存器，A作变址寄存器，相加形成16位的地址来访问程序存储区。例如:MOVC A, @A+DPTR
//● 相对寻址：以程序计数器PC的内容为基地址，加上指令中的偏移量，所得结果为跳转的目标地址。例如: JNZ rel
/// </summary>
/// 
    //指令信息类
    public class InstructInfo
    {
        //字节数
        public ushort bytes;
        //指令周期
        public int cycles;
        //操作数类型
        public OpType op0_mode;
        public OpType op1_mode;
        //指令名
        public string opcode_name;
        //指令操作逻辑
        public virtual void exec(_instruct instr)
        {
            VM_8051_Mono.Instance.PC += instr.info.bytes;
            VM_8051_Mono.Instance.cycles += instr.info.cycles;
        }
    }
    //指令结构类
    public class _instruct
    {
        //操作数
        public byte opcode;
        //操作数0
        public byte op0;
        //操作数1
        public byte op1;
        //指令信息类，包含指令字节数、机器周期和处理逻辑等。
        public InstructInfo info;
    }
    //操作数进行读写的类型
    public enum OpType
    {
        NONE,
        Rn,//寄存器类型 操作数
        IMM8,//8位立即数 操作数
        ACC,
        DIRECT,
        DPTR,
        IMM16,
        R0_R1,
        PSW_Cy,
        BIT,
        ACC_DPTR, // 这两种指令只会发生在读的时候，不会有写的情况
        ACC_PC,
        AT_DPTR,
        AT_SP,
        B
    }
    public enum mem_type
    {
        CODE,
        RAM,
        SFR,
        BIT,
        Exteranl,
    }


