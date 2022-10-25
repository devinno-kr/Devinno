<br />  

# Devinno [![NuGet stable version](https://badgen.net/nuget/v/devinno)](https://nuget.org/packages/devinno)

## 개요
  * 산업용 프로그램에 개발에 필요한 통신 및 기타 기능들을 정리한 라이브러리
    <br />
    <br />  

## 참조
  * [M2MqttDotNetCore](https://github.com/mohaqeq/paho.mqtt.m2mqtt) [1.1.0](https://www.nuget.org/packages/M2MqttDotnetCore/1.1.0)
  * [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis/) [2.6.70](https://www.nuget.org/packages/StackExchange.Redis/2.6.70)
  * [Newtonsoft.Json](https://www.newtonsoft.com/json) [13.0.1](https://www.nuget.org/packages/Newtonsoft.Json/13.0.1)
    <br />
    <br />  

## 목차
  * Devinno.Collection
    * [EventList](#EventList)
  * Devinno.Communications
    * [CNet](#CNet)
    * [MC](#MC)
    * [ModbusRTUMaster](#ModbusRTUMaster)
    * [ModbusRTUSlave](#ModbusRTUSlave)
    * [ModbusTCPMaster](#ModbusTCPMaster)
    * [ModbusTCPSlave](#ModbusTCPSlave)
    * [MQClient](#MQClient)
    * [RedisClient](#RedisClient)
    * [TextCommRTUMaster](#TextCommRTUMaster)
    * [TextCommRTUSlave](#TextCommRTUSlave)
    * [TextCommTCPMaster](#TextCommTCPMaster)
    * [TextCommTCPSlave](#TextCommTCPSlave)
  * Devinno.Data
    * [INI](#INI)
    * [Memories](#Memories)
    * [Serialize](#Serialize)
  * Devinno.Extensions
    * [BitExtension](#BitExtension)
    * [ColorExtension](#ColorExtension)
  * Devinno.Measure
    * [Chattering](#Chattering)
    * [StableMeasure](#StableMeasure)
  * Devinno.Timers
    * [HiResTimer](#HiResTimer)
  * Devinno.Tools
    * [CollisionTool](#CollisionTool)
    * [ColorTool](#ColorTool)
    * [CryptoTool](#CryptoTool)
    * [LauncherTool](#LauncherTool)
    * [MathTool](#MathTool)
    * [NetworkTool](#NetworkTool)
    * [WindowsTool](#WindowsTool)
  * Devinno.Utils
    * [ExternalProgram](#ExternalProgram)
    <br />  
    <br />  

## 사용법
### 1. Devinno.Collection
#### 1.1. EventList
리스트의 변화가 발생시 Changed 이벤트가 발생

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var p1 = new Node { Text = "1" };
    var p2 = new Node { Text = "2" };  p1.Nodes.Add(p2);
    var p3 = new Node { Text = "3" };  p2.Nodes.Add(p3);

    var nd = p3;
    while (nd != null) { Console.WriteLine(nd.Text); nd = nd.Parent; }
}

class Node
{
    public Node Parent { get; set; }
    public EventList<Node> Nodes { get; } = new EventList<Node>();
    public string Text { get; set; }

    public Node() => Nodes.Changed += (o, s) => Nodes.ForEach((x) => x.Parent = this);
}
```

* **결과**
```
3 
2 
1 
```

* **설명** 
``` 
자식 노드들의 변화가 생기면 자식 노드의 부모 노드를 자신으로 지정
```
<br />

### 2. Devinno.Communications
#### 2.1. CNet
LS PLC와 통신할 수 있는 CNET 프로토콜

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var cnet = new CNet { Port = "COM3", Baudrate = 115200 };
    cnet.Start();

    cnet.AutoRSB(10, 1, "%DW000", 4);
    cnet.DataReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.Data) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        cnet.ManualWSS(11, 1, "%DW001", DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  0331  0030  0000  0000 
Slave:1  0334  0030  0000  0000 
Slave:1  0339  0031  0000  0000 
... 
```

* **설명**  
```
국번 1번의 PLC에서 DW000부터 4개 값을 자동으로 읽어오는 명령 등록  
데이터 수신 시 국번과 데이터를 차례대로 출력  
1초 간격으로 현재 초를 DW001에 저장
```
<br />

#### 2.2. MC
Mitsubishi PLC와 통신할 수 있는 MC 프로토콜

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var mc = new MC { Port = "COM3", Baudrate = 115200 };
    mc.Start();

    mc.AutoWordRead(10, 1, "D000", 4);
    mc.WordDataReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.Data) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        mc.ManualWordWrite(11, 1, "D001", DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  002A  000E  0000  0000 
Slave:1  002D  000E  0000  0000 
Slave:1  0030  000E  0000  0000 
... 
```

* **설명**
```    
국번 1번의 PLC에서 D000부터 4개 값을 자동으로 읽어오는 명령 등록
데이터 수신 시 국번과 데이터를 차례대로 출력
1초 간격으로 현재 초를 D001에 저장
```
<br />

#### 2.3. ModbusRTUMaster
산업용 프로토콜 Modbus. ( RTU, Master )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var mb = new ModbusRTUMaster { Port = "COM3", Baudrate = 115200 };
    mb.Start();

    mb.AutoWordRead_FC3(10, 1, 0x7000, 4);
    mb.WordReadReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.ReceiveData) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        mb.ManualWordWrite_FC6(11, 1, 0x7001, DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  0C75  0003  0000  0000 
Slave:1  0C79  0003  0000  0000 
Slave:1  0C7D  0003  0000  0000 
... 
```

* **설명**
```
국번 1번의 PLC에서 0x7000번지 부터 4개 값을 자동으로 읽어오는 명령 등록  
데이터 수신 시 국번과 데이터를 차례대로 출력  
1초 간격으로 현재 초를 0x7001번지에 저장  
```
<br />

#### 2.4. ModbusRTUSlave
산업용 프로토콜 Modbus. ( RTU, Slave )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var D = new WordMemories("D", 1024);

    var mb = new ModbusRTUSlave { Port = "COM4", Baudrate = 115200, Slave = 1 };
    mb.WordAreas.Add(0x7000, D);
    mb.Start();

    while(true)
    {
        D[0] = unchecked((ushort)(D[0] + 1));
        Thread.Sleep(10);
    }
}
```

* **결과**
```
      
```

* **설명**
```
모드버스 국번을 1번으로 지정  
워드 영역 D를 모드버스 워드영역 0x7000번지로 등록  
주기적으로 D0를 1씩 증가  
```      
<br />

#### 2.5. ModbusTCPMaster  
산업용 프로토콜 Modbus. ( TCP, Master )
      
* **샘플코드**
```csharp
static void Main(string[] args)
{ 
    var mb = new ModbusTCPMaster { RemoteIP = "192.168.0.50" }; ;
    mb.Start();

    mb.AutoWordRead_FC3(10, 1, 0x7000, 4);
    mb.WordReadReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.ReceiveData) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        mb.ManualWordWrite_FC6(11, 1, 0x7001, DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  0568  0009  0000  0000 
Slave:1  056B  0009  0000  0000 
Slave:1  056E  0009  0000  0000 
...
```

* **설명**
```
192.168.0.50의 PLC에서 0x7000번지 부터 4개 값을 자동으로 읽어오는 명령 등록  
데이터 수신 시 국번과 데이터를 차례대로 출력  
1초 간격으로 현재 초를 0x7001번지에 저장  
```
<br />

#### 2.6. ModbusTCPSlave  
산업용 프로토콜 Modbus. ( TCP, Slave )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var D = new WordMemories("D", 1024);

    var mb = new ModbusTCPSlave { Slave = 1 };
    mb.WordAreas.Add(0x7000, D);
    mb.Start();

    while (true)
    {
        D[0] = unchecked((ushort)(D[0] + 1));
        Thread.Sleep(10);
    }
}
```

* **결과**
```
          
```

* **설명**
```
모드버스 국번을 1번으로 지정  
워드 영역 D를 모드버스 워드영역 0x7000번지로 등록  
주기적으로 D0를 1씩 증가      
```  
<br />

#### 2.7. MQClient  
MQTT Client 래퍼 ( M2MQTT 기반 )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var mqtt = new MQClient { BrokerHostName = "127.0.0.1" };
    mqtt.Start(Guid.NewGuid().ToString());

    mqtt.Subscribe("/time");
    mqtt.Received += (o, s) => { 
        if (s.Topic == "/time") 
            Console.WriteLine(("R:" + Encoding.UTF8.GetString(s.Datas)).PadLeft(50)); 
    };

    Thread.Sleep(1000);

    while(true)
    {
        mqtt.Publish("/time", Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
        Console.WriteLine(("S:" + DateTime.Now.ToString()).PadRight(50));
        System.Threading.Thread.Sleep(1000);
    }
}
```

* **결과**
```
S:2022-10-24 오후 4:45:15 
                            R:2022-10-24 오후 4:45:15 
S:2022-10-24 오후 4:45:16 
                            R:2022-10-24 오후 4:45:16 
S:2022-10-24 오후 4:45:17 
                            R:2022-10-24 오후 4:45:17 
... 
```

* **설명**
```
/test 구독하고 해당 토픽 수신 시 수신되 데이터 콘솔에 출력
1초에 한 번씩 /test에 현재 시간 문자열을 발행
```
<br />

#### 2.8. RedisClient  
Redis Client 래퍼 ( StackExchange.Redis 기반 )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var redis = new RedisClient { Host = "127.0.0.1" };
    redis.Open();
            
    var tmr = new HiResTimer { Interval = 1000, Enabled = true };
    tmr.Elapsed += (o,s)=> redis.Set("Time", DateTime.Now.ToString("HH:mm:ss.fff"));
    
    redis.Set("Time", DateTime.Now.ToString("HH:mm:ss.fff"));
    while (true)
    {
        Console.WriteLine(redis.GetString("Time"));
        System.Threading.Thread.Sleep(500);
    }
}
```

* **결과**
```
18:13:38.395
18:13:38.395
18:13:39.396
18:13:39.396
18:13:40.396
18:13:40.396
... 
```

* **설명**
```
타이머를 이용해 1초 마다 Time 에 현재 시간을 넣고
루프에선 500ms 마다 Time 정보를 읽어와 콘솔에 출력
결과를 보면 2개 출력마다 값이 변경됨
```
<br />

#### 2.9 TextCommRTUMaster  
텍스트 송/수신 프로토콜 ( RTU, Master )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommRTUMaster { Port = "COM3", Baudrate = 115200, Interval = 100 };
    comm.Start();

    comm.MessageReceived += (o, s) => Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  Data:{s.Message}");
    comm.AutoSend(11, 1, 1, "");

    while (true)
    {
        comm.ManualSend(12, 1, 2, DateTime.Now.ToString("HH:mm:ss.fff"));
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:38
Slave:1  Command:1  Data:Count:39
Slave:1  Command:1  Data:Count:40
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:41
Slave:1  Command:1  Data:Count:42
Slave:1  Command:1  Data:Count:43
Slave:1  Command:1  Data:Count:44
Slave:1  Command:2  Data:OK 
...   
```

* **설명**
```
커맨드 1을 자동으로 전송하도록 등록  
메시지 수신 시 국번, 커맨드, 메시지를 콘솔로 출력  
1초 주기로 커맨드 2번으로 현재 시간을 전송  
```
<br />

#### 2.10 TextCommRTUSlave  
텍스트 송/수신 프로토콜 ( RTU, Slave )
 
* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommRTUSlave { Port = "COM4", Baudrate = 115200 };
    comm.Start();

    var cnt = 0;
    comm.MessageRequest += (o, s) =>
    {
        if (s.Slave == 1)
        {
            switch (s.Command)
            {
                case 1:
                    cnt++;
                    s.ResponseMessage = $"Count:{cnt}";
                    break;
                case 2:
                    Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  {s.RequestMessage}");
                    s.ResponseMessage = "OK";
                    break;
            }
        }
    };

    while (true) Thread.Sleep(100);
}
```

* **결과**
```
Slave:1  Command:2  20:25:00.612
Slave:1  Command:2  20:25:01.625
Slave:1  Command:2  20:25:02.632
Slave:1  Command:2  20:25:03.642
Slave:1  Command:2  20:25:04.656
Slave:1  Command:2  20:25:05.658
Slave:1  Command:2  20:25:06.670
Slave:1  Command:2  20:25:07.684
...   
```

* **설명**
```
MessageRequest 이벤트에서는 요청 메시지를 처리하고 응답 메시지를 지정함  
커맨드 1에서는 카운터를 증가하고 해당 카운터값을 응답함  
커맨드 2에서는 수신된 요청을 콘솔에 출력하고 OK 응답  
```

#### 2.11. TextCommTCPMaster  
텍스트 송/수신 프로토콜 ( TCP, Master )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommTCPMaster { RemoteIP = "192.168.0.55", RemotePort = 1234, Interval = 100 };
    comm.Start();

    comm.MessageReceived += (o, s) => Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  Data:{s.Message}");
    comm.AutoSend(11, 1, 1, "");

    while (true)
    {
        comm.ManualSend(12, 1, 2, DateTime.Now.ToString("HH:mm:ss.fff"));
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:17
Slave:1  Command:1  Data:Count:18
Slave:1  Command:1  Data:Count:19
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:20
Slave:1  Command:1  Data:Count:21
Slave:1  Command:1  Data:Count:22
Slave:1  Command:1  Data:Count:23
...   
```

* **설명**
```
커맨드 1을 자동으로 전송하도록 등록  
메시지 수신 시 국번, 커맨드, 메시지를 콘솔로 출력  
1초 주기로 커맨드 2번으로 현재 시간을 전송  
```
<br />

#### 2.12 TextCommTCPSlave  
텍스트 송/수신 프로토콜 ( TCP, Slave )
 
* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommTCPSlave { LocalPort = 1234 };
    comm.Start();

    var cnt = 0;
    comm.MessageRequest += (o, s) =>
    {
        if (s.Slave == 1)
        {
            switch (s.Command)
            {
                case 1:
                    cnt++;
                    s.ResponseMessage = $"Count:{cnt}";
                    break;
                case 2:
                    Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  {s.RequestMessage}");
                    s.ResponseMessage = "OK";
                    break;
            }
        }
    };

    while (true) Thread.Sleep(100);
}
```

* **결과**
```
Slave:1  Command:2  21:13:05.151
Slave:1  Command:2  21:13:06.170
Slave:1  Command:2  21:13:07.183
Slave:1  Command:2  21:13:08.197
Slave:1  Command:2  21:13:09.211
Slave:1  Command:2  21:13:14.420
Slave:1  Command:2  21:13:15.452
Slave:1  Command:2  21:13:16.455
...   
```

* **설명**
```
MessageRequest 이벤트에서는 요청 메시지를 처리하고 응답 메시지를 지정함  
커맨드 1에서는 카운터를 증가하고 해당 카운터값을 응답함  
커맨드 2에서는 수신된 요청을 콘솔에 출력하고 OK 응답  
```
<br />

### 3. Devinno.Data
#### 3.1. INI  
      
#### 3.2. Memories  
      
#### 3.3. Serialize  
      
### 4. Devinno.Extensions
#### 4.1. BitExtension  
      
#### 4.2. ColorExtension  
      
### 5. Devinno.Measure
#### 5.1. Chattering  
      
#### 5.2. StableMeasure  
      
### 6. Devinno.Timers
#### 6.1. HiResTimer  
      
### 7. Devinno.Tools
#### 7.1. CollisionTool  
      
#### 7.2. ColorTool  
      
#### 7.3. CryptoTool  
      
#### 7.4. LauncherTool  
      
#### 7.5. MathTool  
      
#### 7.6. NetworkTool  
      
#### 7.7. WindowsTool  
      
### 8. Devinno.Utils
#### 8.1. ExternalProgram  
      
<br />
<br />

> # Devinno.Database [![NuGet stable version](https://badgen.net/nuget/v/Devinno.Database)](https://nuget.org/packages/Devinno.Database)
