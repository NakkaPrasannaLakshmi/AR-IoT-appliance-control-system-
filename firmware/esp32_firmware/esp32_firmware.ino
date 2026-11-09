/*
 * Augmented Reality-Enabled Internet of Things (AR-IoT)
 * Appliance Control and Environmental Telemetry Firmware for ESP32
 *
 * Interacts with Blynk IoT Cloud via Virtual Pins:
 * - V0: Relay / Appliance Digital Control (0 = OFF, 1 = ON)
 * - V1: Temperature Telemetry (Celsius)
 * - V2: Humidity Telemetry (% RH)
 */

#include <WiFi.h>
#include <BlynkSimpleEsp32.h>
#include <WiFiManager.h>
#include <DHT.h>

// Blynk Credentials (Replace with your Template ID & Auth Token)
#define BLYNK_TEMPLATE_ID "TMPL8952AC"
#define BLYNK_TEMPLATE_NAME "AR-IoT Appliance Control"
char auth[] = "YOUR_BLYNK_AUTH_TOKEN";

// Sensor Configuration
#define DHTPIN 13
#define DHTTYPE DHT11   // DHT11 / DHT22 compatible
DHT dht(DHTPIN, DHTTYPE);

// Appliance Control Output (Relay or Status LED)
#define RELAY_PIN 2

// Timer for non-blocking periodic sensor transmission
unsigned long previousMillis = 0;
const long interval = 2000;  // Send readings every 2 seconds

void setup() {
  Serial.begin(115200);
  delay(100);

  Serial.println("\n--- Initializing AR-IoT Node ---");
  dht.begin();
  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, LOW); // Default to OFF

  // WiFi Provisioning through captive portal
  WiFiManager wm;
  Serial.println("[*] Connecting to WiFi via WiFiManager...");
  if (!wm.autoConnect("AR-IoT_ESP32_ConfigAP")) {
    Serial.println("[!] Failed to connect to WiFi and timeout reached. Restarting ESP32...");
    ESP.restart();
    delay(1000);
  }

  Serial.println("[+] Connected to WiFi successfully!");
  Serial.print("[+] IP Address: ");
  Serial.println(WiFi.localIP());

  // Connect to Blynk IoT Cloud using acquired WiFi credentials
  Blynk.begin(auth, WiFi.SSID().c_str(), WiFi.psk().c_str());
  Serial.println("[+] Blynk Cloud connection initialized.");
}

void loop() {
  Blynk.run();

  unsigned long currentMillis = millis();
  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;
    sendSensorData();
  }
}

// Read temperature and humidity, then push to Blynk virtual pins
void sendSensorData() {
  float humidity = dht.readHumidity();
  float temperature = dht.readTemperature();

  if (isnan(humidity) || isnan(temperature)) {
    Serial.println("[!] Warning: Failed to read from DHT sensor!");
    return;
  }

  // Push telemetry to Blynk Cloud
  Blynk.virtualWrite(V1, temperature);
  Blynk.virtualWrite(V2, humidity);

  Serial.print("[Telemetry] Temp: ");
  Serial.print(temperature, 1);
  Serial.print(" °C | Humidity: ");
  Serial.print(humidity, 1);
  Serial.println(" %");
}

// Callback invoked when AR App / Blynk updates Virtual Pin V0
BLYNK_WRITE(V0) {
  int relayState = param.asInt();
  digitalWrite(RELAY_PIN, relayState);

  Serial.print("[Action] Appliance State Changed -> ");
  Serial.println(relayState == 1 ? "ON" : "OFF");
}
