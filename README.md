# CPU Scheduling and Cache Performance Simulation

## Overview
This project implements a discrete-event simulation to study the behavior of CPU scheduling algorithms and simplified cache performance models. The simulation is designed to evaluate trade-offs between fairness, responsiveness, and throughput under mixed workloads that include both CPU-bound and I/O-bound processes.

The project was developed as part of **CS 4632** at **Kennesaw State University** and follows an academic simulation-driven approach inspired by classical operating systems and computer architecture literature.

---

## Objectives
- Simulate and compare classical CPU scheduling algorithms
- Analyze scheduling performance metrics such as waiting time and turnaround time
- Model cache behavior using probabilistic abstractions
- Explore interactions between scheduling policies and memory access latency
- Provide a modular foundation for future extensions and experimentation

---

## Features
- **CPU Scheduling Algorithms**
  - First-Come First-Served (FCFS)
  - Round Robin (RR) with configurable time quantum
  - Priority Scheduling (preemptive or non-preemptive)

- **Discrete-Event Simulation**
  - Event-driven system with a global simulation clock
  - Process arrivals, dispatch events, context switches, and completion events

- **Queue-Based Process Management**
  - Ready and waiting queues
  - Explicit modeling of contention for CPU resources

- **Probabilistic Cache Model**
  - Cache hit/miss behavior based on configurable probabilities
  - Cache misses introduce execution delays to simulate memory hierarchy effects

- **Performance Metrics Collection**
  - Average waiting time
  - Turnaround time
  - CPU utilization
  - Cache hit rate
  - Context switch count

---

## System Architecture
The simulation is structured using a modular, object-oriented design:

- **SimulationEngine**  
  Coordinates the simulation clock, event queue, and system components.

- **Scheduler Interface and Implementations**  
  Encapsulates scheduling policy logic (FCFS, RR, Priority).

- **CPU and CacheModel**  
  Models CPU execution and probabilistic memory access behavior.

- **Process and WorkloadGenerator**  
  Represents system workloads and process arrival patterns.

- **MetricsCollector**  
  Tracks and reports performance metrics across simulation runs.

UML class and sequence diagrams describing the architecture and control flow are included in the project documentation.

---

## Technologies Used
- **Programming Language:** C#
- **Simulation Type:** Discrete-event simulation
- **Development Environment:** .NET / Visual Studio
- **Documentation:** LaTeX (ACM `acmart` format)
- **Modeling:** UML (Class and Sequence Diagrams)

---

## Repository Structure
├── src/ # C# simulation source code
├── docs/ # Project paper and documentation
│ ├── CS_4632_Project.pdf
│ └── uml/
│ ├── class_diagram.puml
│ └── sequence_diagram.puml
├── figures/ # Rendered UML diagrams (PNG/SVG)
├── README.md
└── references/ # Bibliography and research notes

---

## How to Run (Planned)
1. Open the project in Visual Studio
2. Configure simulation parameters (scheduler type, time quantum, cache probability)
3. Run the simulation
4. Review generated performance metrics and reports

*(Exact execution instructions will be finalized in later milestones.)*

---

## Assumptions and Limitations
- Single or simplified multi-core CPU model
- Fixed context switch overhead
- No shared memory or synchronization between processes
- Cache modeled probabilistically rather than at the cache-line level
- Focus on high-level system behavior rather than hardware-specific timing

---

## References
This project is informed by established operating systems and computer architecture research, including:

- Silberschatz, Galvin, and Gagne — *Operating System Concepts*
- Tanenbaum and Bos — *Modern Operating Systems*
- Hennessy and Patterson — *Computer Architecture: A Quantitative Approach*
- Agarwal et al. — Analytical cache performance modeling
- Banks et al. — Discrete-event system simulation

Full citations are available in the project paper.

---

## Author
**Joshua Gibson**  
Kennesaw State University  
CS 4632 – Simulation and Modeling

---

## License
This project is intended for educational use as part of a university course.
