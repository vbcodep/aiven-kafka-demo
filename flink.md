## Using Apache Flink to split sensor topic into two streams 



SensorA and SensorB are two flink applications that allow the sensor topic to split the data
into two topics 




### Application SensorA

###  SQL used to create source table from sensor topic

    CREATE TABLE sensors (
    id STRING,
    Name STRING,
    TempF INT,
    Humidity INT,
    LastUpdate STRING
     ) WITH (
    'connector' = 'kafka',
    'properties.bootstrap.servers' = 'kafka-1b96a7b7-isaacjg-d86e.aivencloud.com:15046',
    'scan.startup.mode' = 'earliest-offset',
    'topic' = 'sensor',
    'value.format' = 'json'
    )


### SQL used to create sink table for sensora

    CREATE TABLE sensora (
    id STRING,
    Name STRING,
    TempF INT,
    Humidity INT,
    LastUpdate STRING
     ) WITH (
    'connector' = 'kafka',
    'properties.bootstrap.servers' = 'kafka-1b96a7b7-isaacjg-d86e.aivencloud.com:15046',
    'scan.startup.mode' = 'earliest-offset',
    'topic' = 'sensora',
    'value.format' = 'json'
    )

### SQL Statement for creating new topic
    insert into sensora 
    select * from sensors where Name = 'Weather-Station-A'
    

## Application SensorB

### SQL used to create source table from sensor topic

    CREATE TABLE sensors (
    id STRING,
    Name STRING,
    TempF INT,
    Humidity INT,
    LastUpdate STRING
     ) WITH (
    'connector' = 'kafka',
    'properties.bootstrap.servers' = 'kafka-1b96a7b7-isaacjg-d86e.aivencloud.com:15046',
    'scan.startup.mode' = 'earliest-offset',
    'topic' = 'sensor',
    'value.format' = 'json'
    )


### SQL used to create sink table for sensorb

    CREATE TABLE sensorb (
    id STRING,
    Name STRING,
    TempF INT,
    Humidity INT,
    LastUpdate STRING
     ) WITH (
    'connector' = 'kafka',
    'properties.bootstrap.servers' = 'kafka-1b96a7b7-isaacjg-d86e.aivencloud.com:15046',
    'scan.startup.mode' = 'earliest-offset',
    'topic' = 'sensorb',
    'value.format' = 'json'
    )

### SQL Statement for creating new topic 
    insert into sensorb 
    select * from sensors where Name = 'Weather-Station-B'


