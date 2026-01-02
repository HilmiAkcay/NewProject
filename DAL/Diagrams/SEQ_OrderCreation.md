## Order Creation + Promotion Preview (NO persistence)
```mermaid

sequenceDiagram
    actor User
    participant UI
    participant OrderService
    participant PromotionEngine

    User->>UI: Add / Update Order Line
    UI->>OrderService: Save Order Draft
    OrderService->>OrderService: Persist Order + OrderLines
    OrderService-->>UI: Order Saved

    User->>UI: Request Promotion Preview
    UI->>PromotionEngine: Evaluate(cart_state)
    PromotionEngine-->>UI: PromotionPreview[]
