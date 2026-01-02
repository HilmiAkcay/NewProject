## Order Creation + Promotion Preview (NO persistence)
```mermaid

sequenceDiagram
    actor User
    participant UI
    participant OrderService
    participant PromotionEngine

    User->>UI: Add / Update / Remove OrderLine
    UI->>OrderService: Save Order Draft
    OrderService->>OrderService: Persist Order + OrderLines

    OrderService->>PromotionEngine: Evaluate(cart_state)
    PromotionEngine-->>OrderService: PromotionPreview[]

    OrderService-->>UI: Updated Totals + Promotions
