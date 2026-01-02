## Manual Discount Intent (Order level)
```mermaid

sequenceDiagram
    actor User
    participant UI
    participant OrderService

    User->>UI: Apply Manual Discount
    UI->>OrderService: Create OrderPriceAdjustment
    OrderService->>OrderService: Persist Discount Intent
    OrderService-->>UI: Discount Saved
