# CPU Scheduling and Cache Performance Simulation

## Overview

This project is a discrete-event simulation system designed to model and analyze CPU scheduling behavior and cache performance under varying workloads. The simulation evaluates how different scheduling strategies impact system efficiency, fairness, responsiveness, and resource utilization.

Developed as part of **CS 4632 – Modeling and Simulation** at **Kennesaw State University**, this project combines operating systems theory, stochastic workload modeling, and performance analysis into a configurable simulation framework.

The simulation implements and compares:

* First-Come First-Serve (FCFS)
* Round Robin (RR)
* Priority Scheduling (Preemptive and Non-Preemptive)

It also integrates a probabilistic cache hit/miss model to simulate realistic execution delays.

---

# Project Features

## Core Simulation Framework

* Discrete-event simulation engine
* Event-driven execution model
* Global simulation clock
* Configurable simulation duration
* Automated event processing

---

## Scheduling Algorithms

### First-Come First-Serve (FCFS)

* FIFO process execution
* Simple and predictable scheduling behavior

### Round Robin (RR)

* Configurable time quantum
* Fair CPU distribution
* Context switching support

### Priority Scheduling (Non-Preemptive)

* Higher-priority process selection
* Arrival-order tie-breaking

### Priority Scheduling (Preemptive)

* Dynamic process interruption
* Immediate priority enforcement

---

## Workload Modeling

* Poisson-based process arrivals
* Exponentially distributed interarrival times
* Configurable process generation rate
* Configurable workload size

---

## Cache Performance Simulation

* Probabilistic cache hit/miss modeling
* Configurable hit probability
* Configurable miss penalties
* Cache performance tracking

---

## Metrics Collection

The simulation tracks:

* Average waiting time
* Average turnaround time
* CPU utilization
* Context switch count
* Completed process count
* Cache hits
* Cache misses
* Cache hit rate

---

## Data Export

Simulation results are exported automatically:

* Run summary CSV
* Process logs
* Event logs
* Aggregate performance summaries

---

# Installation

## Requirements

* .NET 8 SDK
* Visual Studio 2022 or newer

OR

* Visual Studio Code + C# Extension

---

## Clone Repository

```bash
git clone https://github.com/yumakuga-afk/CPU-Scheduling-and-Cache-Performance-Simulation.git
cd CPU-Scheduling-and-Cache-Performance-Simulation
```

---

## Build Project

```bash
dotnet build
```

---

## Run Project

```bash
dotnet run
```

---

# Usage

## Scheduler Selection

Scheduler selection is configurable through runtime parameters.

Examples:

Run FCFS:

```bash
dotnet run fcfs
```

Run Round Robin:

```bash
dotnet run rr
```

Run Priority Scheduling:

```bash
dotnet run priority
```

---

## Configuration Parameters

Simulation behavior is controlled using configurable parameters:

* EndTime
* ProcessCountTarget
* MeanInterarrivalTime
* CacheHitProbability
* CacheMissPenalty
* ContextSwitchCost
* TimeQuantum
* RandomSeed

Example:

```csharp
var config = new SimConfig
{
    EndTime = 500,
    ProcessCountTarget = 100,
    MeanInterarrivalTime = 0.5,
    CacheHitProbability = 0.85,
    CacheMissPenalty = 3,
    TimeQuantum = 8
};
```

---

# Example Output

Example run summary:

```text
Scheduler: Priority Scheduling (Preemptive)
Completed Processes: 42
Average Wait Time: 108.71
Average Turnaround Time: 119.19
CPU Utilization: 91.6%
Context Switches: 45
Cache Hit Rate: 92.41%
Cache Misses: 6
```

---

# Experimental Results Summary

Key findings from 15 simulation runs:

## Best Waiting Time

FCFS: 97.76

## Best Turnaround Time

FCFS: 108.55

## Best CPU Utilization

Priority Scheduling: 93.2%

## Highest Context Switching

Round Robin: 76

## Best Cache Hit Rate

Round Robin: 93.02%

## Overall Best Performer

Priority Scheduling (based on consistency)

---

# System Architecture

Core system flow:

Process Generation
→ Scheduler Selection
→ CPU Execution
→ Cache Simulation
→ Metrics Collection
→ Data Export

---

## Core Components

### SimulationEngine

Controls simulation execution and event flow.

### Process

Represents workload entities.

### Scheduler Interface

Defines scheduling contract.

Implemented by:

* FCFSScheduler
* RoundRobinScheduler
* PriorityScheduler

### CacheSimulator

Handles cache hit/miss behavior.

### MetricsCollector

Tracks and computes performance metrics.

### DataExporter

Exports simulation output.

---

# UML Diagrams

Located in:

```text
/docs/uml/
```

Includes:

* Class diagrams
* Architecture diagrams

---

# Repository Structure

```text
├── src/
│   ├── Schedulers/
│   ├── Simulation/
│   ├── Cache/
│   ├── Models/
│   └── Export/
├── docs/
│   ├── Final_Report.pdf
│   └── uml/
├── output/
│   ├── run_summaries/
│   ├── event_logs/
│   └── process_logs/
├── figures/
└── README.md
```

---

# Assumptions and Limitations

* Single CPU simulation
* Simplified probabilistic cache model
* Synthetic workloads
* Fixed context switch overhead
* No multicore support

---

# Future Improvements

Planned future enhancements:

* Shortest Job First (SJF)
* Shortest Remaining Time First (SRTF)
* Multilevel Queue Scheduling
* Multilevel Feedback Queue Scheduling
* Multicore CPU support
* Advanced cache hierarchy modeling
* GUI visualization
* Real-world workload traces

---

# Documentation

Project documentation includes:

* Final LaTeX report
* UML diagrams
* Performance graphs
* Experimental results

---

# Author

Joshua Gibson
Kennesaw State University
CS 4632 – Modeling and Simulation

---

# License

Academic use only.
