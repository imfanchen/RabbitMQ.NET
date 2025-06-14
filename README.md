# Introduction

This repository contains sample C#/.NET code for using RabbitMQ.

# Installation

To run a RabbitMQ instance for development, the best approach is to use a Docker container.

Ensure you have the latest version of Docker Desktop installed on your machine.

Then, run the following command in your terminal:

```bash
docker run --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management
```

Afterward, you can log into the RabbitMQ Management interface from your browser at [http://localhost:15672/](http://localhost:15672/).

# Description

RabbitMQ offers multiple ways to handle messaging. It is initially designed with the AMQP-compliant messaging protocol.

RabbitMQ is excellent for decoupling producers and consumers of messages through RabbitMQ brokers.

The broker's primary role is to forward messages from the producer to the consumer.

Here are some basic usage patterns for RabbitMQ:

1. **Basic Queue**: A simple queue where a sender sends messages to a receiver.
2. **Work Queue**: A round-robin approach to delegate tasks from one manager to multiple workers for long-running tasks.
3. **Publish/Subscribe Pattern**: A producer publishes messages to an exchange, and the exchange fans out messages to multiple queues that consumers subscribe to.
4. **Routing Pattern**: A producer publishes messages to an exchange, and the exchange directs messages to multiple queues based on a routing key.
5. **Topic Pattern**: A producer publishes messages to an exchange, and the exchange forwards messages to multiple queues based on topics (wildcard routing keys).

For more details, please visit the [RabbitMQ documentation](https://www.rabbitmq.com/docs/use-rabbitmq).

# Usage

Navigate to the subfolder containing the `Program.cs` file for the desired example.

Then, run the following command:

```bash
dotnet run [command-line-arguments]
```

Ensure you have the latest version of the .NET SDK installed on your machine.