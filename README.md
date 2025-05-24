# Equity Positions Management Web Application

## Overview

This web application is designed to maintain and track Equity positions based on a stream of trade transactions. The system ingests transactions with different actions (INSERT, UPDATE, CANCEL) and calculates the resulting positions for each security.

---

## Features

- **Transaction Management:**  
  Supports INSERT, UPDATE, and CANCEL operations on trades with version control.

- **Position Calculation:**  
  Positions are updated dynamically based on the latest version of each trade.

- **Resilient Ordering:**  
  Handles out-of-sequence transaction arrivals gracefully.

- **REST API:**  
  Backend API built using ASP.NET Core for managing transactions and fetching current positions.

- **Frontend:**  
  Angular-based UI to input transactions and display updated equity positions.

- **In-Memory Database:**  
  Uses an in-memory database for rapid prototyping and testing (can be switched to any DB).
