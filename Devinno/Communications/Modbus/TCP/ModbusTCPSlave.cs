using Devinno.Communications.Setting;
using Devinno.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Modbus.TCP
{
    public class ModbusTCPSlave
    {
        #region Properties
        public int Slave { get; set; } = 1;
        public int LocalPort { get => modbus.LocalPort; set => modbus.LocalPort = value; }
        public bool IsStart => modbus.IsStart;

        public Dictionary<int, BitMemories> BitAreas { get; } = new Dictionary<int, BitMemories>();
        public Dictionary<int, WordMemories> WordAreas { get; } = new Dictionary<int, WordMemories>();
        #endregion

        #region Event
        public event EventHandler<ModbusTCPSlaveBase.SocketEventArgs> SocketConnected;
        public event EventHandler<ModbusTCPSlaveBase.SocketEventArgs> SocketClosed;
        #endregion

        #region Member Variable
        private ModbusTCPSlaveBase modbus;
        #endregion

        #region Constructor
        public ModbusTCPSlave()
        {
            modbus = new ModbusTCPSlaveBase();

            modbus.BitReadRequest += Modbus_BitReadRequest;
            modbus.WordReadRequest += Modbus_WordReadRequest;
            modbus.BitWriteRequest += Modbus_BitWriteRequest;
            modbus.WordWriteRequest += Modbus_WordWriteRequest;
            modbus.MultiBitWriteRequest += Modbus_MultiBitWriteRequest;
            modbus.MultiWordWriteRequest += Modbus_MultiWordWriteRequest;
            modbus.WordBitSetRequest += Modbus_WordBitSetRequest;

            modbus.SocketConnected += (o, s) => SocketConnected?.Invoke(this, s);
            modbus.SocketClosed += (o, s) => SocketClosed?.Invoke(this, s);
        }
        #endregion

        #region EventHandler
        private void Modbus_BitReadRequest(object sender, ModbusTCPSlaveBase.BitReadRequestArgs args)
        {
            foreach (var BaseAddress in BitAreas.Keys)
            {
                var mem = BitAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    var ret = new bool[args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        var sidx = args.StartAddress - BaseAddress + i;
                        ret[i] = mem[sidx];
                    }

                    args.ResponseData = ret;
                    args.Success = true;
                }
            }
        }

        private void Modbus_WordReadRequest(object sender, ModbusTCPSlaveBase.WordReadRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    var ret = new int[args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        var sidx = args.StartAddress - BaseAddress + i;
                        ret[i] = mem[sidx];
                    }
                    args.ResponseData = ret;
                    args.Success = true;
                }
            }

        }

        private void Modbus_BitWriteRequest(object sender, ModbusTCPSlaveBase.BitWriteRequestArgs args)
        {
            foreach (var BaseAddress in BitAreas.Keys)
            {
                var mem = BitAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + mem.Size)
                {
                    mem[args.StartAddress - BaseAddress] = args.WriteValue;
                    args.Success = true;
                }
            }
        }

        private void Modbus_WordWriteRequest(object sender, ModbusTCPSlaveBase.WordWriteRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + mem.Size)
                {
                    mem[args.StartAddress - BaseAddress] = args.WriteValue;
                    args.Success = true;
                }
            }
        }

        private void Modbus_MultiBitWriteRequest(object sender, ModbusTCPSlaveBase.MultiBitWriteRequestArgs args)
        {
            foreach (var BaseAddress in BitAreas.Keys)
            {
                var mem = BitAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    for (int i = 0; i < args.WriteValues.Length; i++) mem[args.StartAddress - BaseAddress + i] = args.WriteValues[i];
                    args.Success = true;
                }
            }
        }

        private void Modbus_MultiWordWriteRequest(object sender, ModbusTCPSlaveBase.MultiWordWriteRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    for (int i = 0; i < args.WriteValues.Length; i++) mem[args.StartAddress - BaseAddress + i] = args.WriteValues[i];
                    args.Success = true;
                }
            }
        }

        private void Modbus_WordBitSetRequest(object sender, ModbusTCPSlaveBase.WordBitSetRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + mem.Size && (args.BitIndex >= 0 && args.BitIndex < 16))
                {
                    var p = Convert.ToUInt16(Math.Pow(2, args.BitIndex));
                    if (args.WriteValue) mem[args.StartAddress - BaseAddress] |= p;
                    else mem[args.StartAddress - BaseAddress] &= (ushort)~p;

                    args.Success = true;
                }
            }
        }
        #endregion

        #region Method
        public void Start() => modbus.Start();
        public void Stop() => modbus.Stop();
        #endregion
    }
}
