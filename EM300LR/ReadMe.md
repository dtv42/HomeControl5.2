# EM300LR

The EM300LR projects are comprised of a library, a console application, a test project, and a web application.
The b-control energy meter is equipped with a REST based web services allowing the monitoring of a variety of measured values.

Note that the b-control energy meter also provides a Modbus TCP interface that allows access to various data values.

## EM300LRApp

The .NET core console application allows to read data values from a b-control energy meter using the EM300Lib for the communication and data definitions.

## EM300LRLib

This library provides access to various data fields from the EM300LR web service.
The data properties are based on the specification *"EM300LR Solar API V1 (42,0410,2012,EN 008-15092016)"*.

## EM300LRTest

The testing classes include test for the EM300LR library, the EM300LR console application and the EM300LR web application including SignalR client tests.

## EM300LRWeb

The asp.net core web application provides a web api on top of the EM300LR web service. A web api is provided to access measured values.
