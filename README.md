# Fabric 🧵

**Fabric** is a proof-of-concept (POC) combinator library for calculating the value of financial contracts. Inspired by the need for flexibility in deal-making, Fabric enables users to define contracts as scripts—removing the bottleneck of waiting for IT to implement new deal types.

---

## 💡 Why Fabric?

Traditional financial systems lock you into a fixed set of contract templates. If you need to structure a new type of deal, you're stuck waiting for engineering. With Fabric, you're free to define contracts directly in a scripting format (borrowing C#-like syntax), enabling rapid exploration and execution.

---

## 🚀 How It Works

The calculation flow follows three main steps:

1. **Parsing**:  
   The contract script is interpreted as a C# expression.
   
2. **AST Construction**:  
   The raw expression tree is transformed into a **Fabric Abstract Syntax Tree (AST)**—a structured representation of the contract.
   
3. **Evaluation**:  
   The Fabric AST is traversed to compute the **contract's value**, which is returned to the user.

---

## ✍️ Example

To get started:

1. Navigate to:  
   [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)
   
2. Use the `/contract/value` endpoint to **POST** a contract script.  
   Example payload:

   ```
   {
      "contract": "And(Pay(\"2025-04-30\", 1000000, \"EUR\"), Receive(\"2025-04-30\", 7500000, \"DKK\"))"
   }
   ```

   This script defines a contract where one leg pays a certain amount and the other leg receives another—both on the same date.

---

## 📦 What’s Included

- A parser for C#-style contract expressions  
- A Fabric AST builder  
- An evaluator that computes contract values from the AST  
- A simple web API for input and output (Swagger UI)

---

## ⚠️ What’s *Not* Included (Yet)

This is a **proof of concept**—keep expectations accordingly. The following features are **not included**:

- A comprehensive set of financial combinators  
- Sophisticated financial models (yield curves, discounting, etc.)  
- Security hardening or production readiness  

---

## 🧪 Status

Fabric is currently in a prototype stage and primarily serves as a foundation for experimentation, learning, or further extension.
