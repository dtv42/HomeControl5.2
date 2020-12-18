# ETAPU11

The ETAPU11 projects are comprised of a library, a console application, a test project, and a web application.
The ETA PU 11 pellets boiler is equipped with a Modbus based interface allowing the monitoring of a variety of measured values.

Note that a REST based web interface also allows access to various data values (see EM300LR projects).
The ETA PelletsUnit is equipped with a control system for the entire heating system providing remote control via the meinETA communication platform and REST based web services allowing the monitoring of a variety of combustion parameters and measured values.

## ETAPU11App

The .NET core console application allows to read data values from a b-control energy meter using the EM300Lib for the communication and data definitions.

## ETAPU11Lib

This library provides access to various data fields from the ETAPU11 Modbus interface.
The data properties are based on the specification *"ETAtouch RESTful Webservices" Version 1.1 November 8, 2012*.

## ETAPU11Test

The testing classes include test for the ETAPU11 library, the ETAPU11 console application and the ETAPU11 web application including SignalR client tests.

## ETAPU11Web

The asp.net core web application provides a web api on top of the ETAPU11 web service. A web api is provided to access measured values.

