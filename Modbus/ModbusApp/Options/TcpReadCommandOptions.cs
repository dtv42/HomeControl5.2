namespace ModbusApp.Options
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.IO;

    using ModbusLib;

    #endregion

    public class TcpReadCommandOptions : TcpCommandOptions
    {
        public bool Coil { get; set; }
        public bool Discrete { get; set; }
        public bool Holding { get; set; }
        public bool Input { get; set; }
        public bool Hex { get; set; }
        public ushort Number { get; set; } = 1;
        public ushort Offset { get; set; } = 0;
        public string Type { get; set; } = string.Empty;

        /// <summary>
        ///  Additional check on 
        /// </summary>
        /// <returns></returns>
        public void CheckOptions(IConsole console)
        {
            if ((Coil || Discrete) && Hex)
            {
                console.Out.WriteLine("HEX output option is ignored.");
            }

            if (Input || Holding)
            {
                var message = Type.ToLower() switch
                {
                    "bits" => (Number > 1) ? "Only a single bit array value is supported." : null,
                    "string" => null,
                    _ => Hex ? "HEX output option is ignored." : null
                };

                if (!string.IsNullOrEmpty(message)) console.Out.WriteLine(message);
            }

            if (!string.IsNullOrEmpty(Type) && (Coil || Discrete))
            {
                console.Out.WriteLine($"Specified type '{Type}' is ignored.");
            }

            if (Coil || Discrete)
            {
                if ((Number < 1) || (Number > IModbusClient.MaxBooleanPoints))
                {
                    throw new ArgumentOutOfRangeException($"Number {Number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                }
            }

            if (Holding || Input)
            {
                if (!string.IsNullOrEmpty(Type))
                {
                    _ = Type switch
                    {
                        "bits" => (Number > 1) ? throw new ArgumentOutOfRangeException($"{nameof(Number)}", "Only a single bit array value is supported.") : true,
                        "string" => ((Number < 1) || ((Number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading string values: Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "byte" => ((Number < 1) || ((Number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading byte values:   Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "short" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading short values:  Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "ushort" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading ushort values: Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "int" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading int values:    Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "uint" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading uint values:   Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "float" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading float values:  Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "double" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading double values: Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "long" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading long values:   Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "ulong" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading ulong values:  Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        _ => throw new ArgumentOutOfRangeException($"Unknown type '{Type}' (should not happen).")
                    };
                }
                else
                {
                    if ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints))
                    {
                        throw new ArgumentOutOfRangeException($"Number {Number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                    }
                }
            }
        }
    }
}
