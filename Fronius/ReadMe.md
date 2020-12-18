# Fronius

This library provides access to various data fields from the Fronius Symo 8.2-3-M inverter.
The data properties are based on the specification Fronius Solar API V1 (42,0410,2012,EN 008-15092016).

With power categories ranging from 3.0 to 8.2 kW, the transformerless Fronius Symo is a three-phase solar inverter.
The high system voltage, wide input voltage range and two MPP trackers ensure maximum flexibility in system design.
The standard interface to the internet via WLAN or Ethernet make the Fronius Symo easy to integrate.

*Integrated WLAN interface*: With the Fronius Datamanager, the Fronius Symo offers a WLAN interface in the inverter itself. The inverter is connected to the internet without additional cabling and support the monitoring of the PV system and its operation.

*Open data communication*: The open Modbus TCP SunSpec standard protocol provides a simple way of establishing a data connection to other systems. The protocol is used via the existing Ethernet interface, guaranteeing reliable communication. Additionally a REST based Web service interface allows simple access to the inverter data.

#### Fronius REST Web Service API
The REST Web Service of the Fronius Symo provides access to various data sets:

- Inverter Info
- Logger Info
- Common Data
- Phase Data
- MinMax Data

##### InverterInfo
The InverterInfo class holds the following properties:

- Index
- DeviceType
- PVPower
- CustomName
- Show
- UniqueID
- ErrorCode
- StatusCode

##### LoggerInfo
The LoggerInfo class holds the following properties:

- UniqueID
- ProductID
- PlatformID
- HWVersion
- SWVersion
- TimezoneLocation
- TimezoneName
- UTCOffset
- DefaultLanguage
- CashFactor
- CashCurrency
- CO2Factor
- CO2Unit

##### CommonData
The CommonData class holds the following properties:

- Frequency
- CurrentDC
- CurrentAC
- VoltageDC
- VoltageAC
- PowerAC
- DailyEnergy
- YearlyEnergy
- TotalEnergy
- StatusCode

##### PhaseData
The PhaseData class holds the following properties:

- CurrentL1
- CurrentL2
- CurrentL3
- VoltageL1N
- VoltageL2N
- VoltageL3N

##### MinMaxData
The MinMaxData class holds the following properties:

- DailyMaxVoltageDC
- DailyMaxVoltageAC
- DailyMinVoltageAC
- YearlyMaxVoltageDC
- YearlyMaxVoltageAC
- YearlyMinVoltageAC
- TotalMaxVoltageDC
- TotalMaxVoltageAC
- TotalMinVoltageAC
- DailyMaxPower
- YearlyMaxPower
- TotalMaxPower

#### Fronius Class
The *Fronius* class implements all communication routines to read the data values from the Fronius Symo inverter.
The only data needed to instanciate a *Fronius* class instance are:

- A logger
- The base uri
- The device id

The *Fronius* class provides simple Read() methods to update the data values.
Additional property helper functions allow to read properties or get property values.