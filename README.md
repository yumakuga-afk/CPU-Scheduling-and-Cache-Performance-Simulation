# CPU Scheduling and Cache Performance Simulation

## Overview
This project implements a discrete-event simulation to study the behavior of CPU scheduling algorithms and simplified cache performance models. The simulation evaluates trade-offs between fairness, responsiveness, and throughput under mixed workloads that include both CPU-bound and I/O-bound processes.

Developed as part of **CS 4632 – Simulation and Modeling** at **Kennesaw State University**, this project integrates classical operating systems theory with discrete-event simulation techniques.

---

# Project Status

## ✅ Implemented So Far

### Core Simulation Framework
- Discrete-event simulation engine
- Chronological event queue
- Global simulation clock
- Event-driven execution model

### Scheduling Algorithms
- First-Come First-Served (FCFS)
- Round Robin (RR) with configurable time quantum

### Memory Hierarchy Model
- Probabilistic cache hit/miss model
- Configurable hit probability
- Configurable miss penalty delay
- Cache performance metrics collection

### Metrics Collection
- Average waiting time
- Average turnaround time
- CPU utilization
- Context switch count
- Cache hit rate and miss count

---

## 🚧 Still To Come

- Priority Scheduling implementation
- CSV export for performance analysis
- Command-line configuration support
- Multi-core simulation support
- Extended workload modeling (Poisson/exponential arrivals)
- Visualization or graph generation for performance comparison

---

## 🔄 Changes from Original Proposal

- Cache modeling simplified to a probabilistic delay model rather than detailed cache-line simulation
- Initial version uses deterministic interarrival ranges instead of full stochastic distributions
- Priority scheduling deferred to later milestone

These changes were made to ensure a working baseline implementation while preserving extensibility.

---

# Installation Instructions

## Dependencies

- .NET 8.0 SDK (recommended)
- Visual Studio 2022 (or newer)
  OR
- Visual Studio Code with C# extension

### Verify .NET Installation
Run:
dotnet --version


You should see version 8.x.x or similar.

---

## Step-by-Step Setup

### Using Visual Studio

1. Clone or download the repository
2. Open `CpuSchedulingSim.sln`
3. Build the solution
4. Run the project

---

## Troubleshooting

### Issue: `dotnet not recognized`
Install the .NET SDK from:
https://dotnet.microsoft.com/download

### Issue: Build errors about missing SDK
Ensure the correct .NET version is installed and selected.

### Issue: No output or simulation ends immediately
Check configuration values in `Program.cs`:
- EndTime
- ProcessCountTarget
- CacheHitProbability

---

# Usage

## Running the Simulation

The simulation runs from `Program.cs`.

To switch scheduling algorithms:

```csharp
IScheduler scheduler = new RoundRobinScheduler(timeQuantum: 4);
// OR
IScheduler scheduler = new FCFSScheduler();
```
Then run:

dotnet run

Configuration

Simulation parameters are controlled via SimConfig:

- EndTime

- ContextSwitchCost

- CacheHitProbability

- CacheMissPenalty

- MeanInterarrivalTime

- ProcessCountTarget

Example:

```csharp
var config = new SimConfig
{
    EndTime = 500,
    CacheHitProbability = 0.85,
    CacheMissPenalty = 3
};
```

## Expected Output

Console output includes:

- Scheduler name

- Completed processes

- Average wait time

- Average turnaround time

- CPU utilization

- Context switches

- Cache hit rate

- Cache miss count

Example output:

Scheduler: Round Robin (q=4)
Completed Processes: 48

```csharp
Avg Wait Time: 12.53
Avg Turnaround Time: 22.91
CPU Utilization: 87.42%
Context Switches: 73
Cache Hit Rate: 84.31%
Cache Misses: 112
```
# Architecture Overview

The simulation follows a modular object-oriented architecture aligned with the UML diagrams included in docs/uml/.

## Core Components
### SimulationEngine

- Controls the simulation clock

- Processes events from EventQueue

- Coordinates scheduling and cache interaction

### EventQueue

- Maintains events sorted by simulation time

- Drives discrete-event execution

### IScheduler Interface

- Defines scheduling contract

- Implemented by:

   - FCFSScheduler

   - RoundRobinScheduler

### SimProcess

- Represents workload entities

- Tracks arrival time, burst time, priority, memory access rate

### CacheModel

- Implements probabilistic hit/miss behavior

- Injects execution delay on misses

### MetricsCollector

- Tracks performance metrics

- Computes averages and utilization

# Mapping to UML Design

The implementation directly corresponds to the UML diagrams:

- SimulationEngine → Central controller in class diagram

- IScheduler → Strategy pattern for scheduling

- EventQueue → Event-driven architecture (sequence diagram)

- CacheModel → Hardware abstraction layer

- MetricsCollector → Reporting component

Minor architectural refinements were made during implementation to:

- Simplify event type handling

- Reduce unnecessary state transitions

- Improve modular separation between scheduling and cache logic

These changes preserved the conceptual UML design while improving code clarity.

# Technologies Used

- Programming Language: C#

- Framework: .NET 8

- Simulation Type: Discrete-event simulation

- IDE: Visual Studio

- Documentation: LaTeX (ACM format)

- Modeling: UML (Class + Sequence Diagrams)

# Repository Structure
```
├── src/               # C# simulation source code
├── docs/              # Project paper and documentation
│   ├── CS4632_Joshua_Gibson_M1.pdf
│   └── uml/
│       ├── class_diagram.puml
│       └── sequence_diagram.puml
├── figures/           # Rendered UML diagrams
└── README.md
```
# Assumptions and Limitations

- Single CPU model

- Fixed context switch overhead

- No inter-process communication

- Cache modeled probabilistically (not hardware-accurate)

- Focus on comparative performance trends rather than cycle-accurate simulation

# Author

Joshua Gibson
Kennesaw State University
CS 4632 – Simulation and Modeling

# License

This project is intended for academic use only.
