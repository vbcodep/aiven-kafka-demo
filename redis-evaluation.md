### Avian Redis Offering

#### Summary

Setup Business 28 plan and integrated with Grafana using Influx Service.  4 CPU 28GB Ram with HA. 

    Cost of Evaluated Service:
    $950 Redis
    $ 95 Influx
    $ 45 Month
    
    Total Cost: $1090 month.



Pros:  Overall setup was very easy and integration with pre-built Grafana dashboards was very simple.  The additional cost for the Influx and Grafana integration could be also used with other services. Seemless integration into Aiven platform.   Simple pricing plans that include internal network charges.    

Cons: The Redis offering is very basic and would probably be suitable for caching workloads but lacks many features needed for high performance workloads.  

#### Notes
 
1. Single sharded implementation with HA stand by.   Does not appear to support clustering.   

1. Single Tenant database per plan.  

1. Supports HA using addition replicas.

1. Supports VPC, Peering. AWS Private Link.  Transit Gateway?  Private Service Connect? 

1. Persistence.  Snapshots only.  No AOF. This is probably due to fact the there is no imposed limitations on shard sizes so recovery via AOF could be very slow.     

1. Snapshots appear to be enabled on primary shard. 

1. Being able to start/stop is a useful feature.    

1. Does not appear to support client side TLS. 

1. Version 7.0.11

1. Advance setup allows customization of some redis.conf parameters. 

1. Unclear how long it takes for HA to failover. 

1. Migration via OSS replicaof. Mentions using scan if replicaof does not work but this may not be suitable for a live migration. 

1. Two nodes.  Appears to be multi-AZ.  

1. No Additional Network Charges for service on Aiven platform.  

1. Failover seems to be handled by proprietary Aiven process.  

1. Very easy to integrate Grafana but Redis Metrics are very limited.  Do not appear to provide latency metrics. 

1. No SLA support for LUA.  This is probably not a bad thing since most users do not understand the purpose of LUA nor the single threaded architecture of Redis.

1. According to documentation does not appear to be able to support high write traffic or large keys out of the box.

> In general, writing on average 5 megabytes per second works in most cases, and writing on average 50 megabytes per second will almost always result in failure, especially if memory usage nears maximum allowed or if a new node needs to initialize itself.


Note found in documentation. 

> If you have a use case where you need to constantly write large amounts of data you can contact Aiven support to discuss options for your service configuration.


