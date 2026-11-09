# 🔌 Hardware Wiring & Pinout Specifications

This document outlines the wiring configurations between the **ESP32 NodeMCU Development Board**, the **DHT11/DHT22 Environmental Sensor**, the **1-Channel 5V Relay Module**, and the **AC Appliance Load**.

---

## 1. Pin Mapping Table

| Component | Component Pin | ESP32 Pin | Function / Description |
|:---|:---:|:---:|:---|
| **DHT11 / DHT22** | VCC | `3V3` | Power Supply (3.3V) |
| **DHT11 / DHT22** | GND | `GND` | Ground Reference |
| **DHT11 / DHT22** | DATA | `GPIO 13` | One-Wire Digital Telemetry Bus |
| **1-Channel Relay** | VCC | `VIN` (5V) | Relay Coil Operating Power |
| **1-Channel Relay** | GND | `GND` | Common Ground Reference |
| **1-Channel Relay** | IN | `GPIO 2` | Digital Control Signal (Active High) |
| **Status Indicator** | Built-in LED | `GPIO 2` | Visual verification of relay state |

---

## 2. Relay & Mains AC Load Wiring

> ⚠️ **High Voltage Safety Caution:** Always disconnect mains AC power before making or altering connections to the relay terminal blocks.

```text
[ Mains AC Live (230V) ] -----> [ Relay Common (COM) ]
[ Relay Normally Open (NO) ] -> [ AC Appliance (Bulb/Fan) Live Terminal ]
[ AC Appliance Neutral ] -----> [ Mains AC Neutral ]
```

- When `GPIO 2` goes **HIGH (1)**: The relay energizes, closing the `COM` and `NO` circuit, powering on the light bulb.
- When `GPIO 2` goes **LOW (0)**: The relay de-energizes, breaking the connection and turning the light bulb off.

---

## 3. Blynk Virtual Pin Map

| Virtual Pin | Direction | Data Type | Usage |
|:---:|:---:|:---:|:---|
| `V0` | AR App → ESP32 | Integer (`0` or `1`) | Digital Actuation (Relay ON / OFF) |
| `V1` | ESP32 → AR App | Float / String | Temperature Telemetry (°C) |
| `V2` | ESP32 → AR App | Float / String | Humidity Telemetry (% RH) |
