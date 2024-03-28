# Supervisor Consumer Pattern Implementation

This repository contains a C# implementation of the Supervisor Consumer Pattern, a concept introduced by Mark Richards in his course "Mastering Patterns in Event-Driven Architecture." The pattern involves dynamic adjustment of consumer instances based on workload, managed by a supervisor entity. While this implementation doesn't strictly adhere to the Actor Model, it shares conceptual similarities, particularly in autonomy and message-based communication.

## Overview

The implementation is comprised of three core components:

- **AMQPSender**: Sends messages to a RabbitMQ queue, simulating the generation of workloads.
- **AMQPSupervisor**: Dynamically manages consumer instances, adjusting the count based on the queue's message backlog.
- **AMQPConsumer**: Processes messages from the RabbitMQ queue, with each consumer operating independently.
- **AMQPMonitor**: Monitors and logs the current number of active consumers and the pending message count in the queue.

## Actor Model Similarities

- **Component Autonomy**: Similar to actors, both the supervisor and consumers operate independently, focusing on specific responsibilities.
- **Message Passing**: Communication is facilitated through RabbitMQ, resembling the actor model's message-passing mechanism. However, this is broker-mediated rather than direct actor-to-actor.
- **Dynamic Lifecycle Management**: The supervisor's ability to dynamically control consumer instances echoes the actor model's flexibility in creating and terminating actors based on system demands.

## Setup and Configuration

### Prerequisites

- .NET SDK
- RabbitMQ Server

### Running the Application

1. **Start RabbitMQ Server**: Ensure the RabbitMQ server is operational and reachable.

2. **Run AMQPSender**:
   dotnet run --project AMQPSender
   Input the desired number of messages to be sent when prompted.
   
3. **Launch AMQPSupervisor**:
   dotnet run --project AMQPSupervisor
Configure the supervisor's operational parameters as prompted, such as keep-alive duration and initial consumer count.
AMQPConsumer: Consumer instances are autonomously managed by the AMQPSupervisor, necessitating no separate initiation.

4. **Start AMQPMonitor:**
   dotnet run --project AMQPMonitor
This component continuously displays the current number of consumers and the queue's message count.


![supervisor consumer pattern](https://github.com/DanRegheni/supervisor-consumer-pattern/assets/7289123/da1b127e-47c3-4b95-9c77-42be52ef8003)


