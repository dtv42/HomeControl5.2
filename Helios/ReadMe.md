# Helios

This library provides access to various data field from the Helios KWL EC 200 ventilation unit.
The data properties are based on the specification "*Helios Ventilatoren Funktions- und Schnittstellenbeschreibung*"
NR. 82 269 - Modbus Gateway TCP/IP mit EasyControls. Druckschrift-Nr. 82269/07.14 and the Web based interface of the integrated easyControls web service.

The Helios KWL EC 200 is a compact central ventilation unit with heat recovery for central ventilation.

*Remote control*: Equipped with Helios easyControl, a control concept supporting network connection and a Web browser interface.

*Modbus TCP*: A Modbus TCP interface allows remote access to read and write data.

#### Helios REST Web Service API
The REST Web Service of the Helios gateway provides access to various data sets:

- Booster Data
- Device Data
- Display Data
- Error Data
- Fan Data
- Heater Data
- Info Data
- Operation Data
- Sensor Data
- System Data
- Technical Data
- Vacation Data

##### Data Sets
The various data classes hold a subset of the Data properties -
so all properties of the subset can be found in the main data instance.

The data properties also can be retrieved by using the Helios label:

    vXXXXX      - five digits (including leading zeros)

#### Helios Class
The *Helios* class implements all communication routines to read the data values from the Helios Symo inverter.
The only data needed to instanciate a *Helios* class instance are:

- A logger
- The base uri
- The password

The *Helios* class provides simple Read() methods to update the data values.
Additional property helper functions allow to read properties or get property values.