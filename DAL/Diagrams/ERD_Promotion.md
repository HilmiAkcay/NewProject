- ActionType: "PERCENT_DISCOUNT | FIXED_DISCOUNT | FREE_ITEM | FIXED_PRICE"
- ConditionType: "MIN_QTY | MIN_AMOUNT | PRODUCT | CATEGORY | BRAND | CUSTOMER | PRICE_GROUP"
```mermaid
erDiagram

    Promotion ||--o{ PromoCondition : has
    Promotion ||--o{ PromoAction : executes
    Promotion ||--o{ PromoUsageLog : tracked_by
    Promotion ||--o{ PromotionCustomer : limited_to
    Promotion ||--o{ PromotionProduct : limited_to

    Promotion {
        int Id PK
        string Code
        string Name
        string Type "B2C_PROMO | B2B_AGREEMENT"
        int Priority
        boolean IsStackable
        datetime StartDate
        datetime EndDate
        string Status "Draft | Active | Paused | Expired"
        datetime CreatedAt
    }

    PromoCondition {
        int Id PK
        int PromotionId FK
        string ConditionType 
        string Operator "= | >= | IN"
        string Value "string or JSON"
    }

    PromoAction {
        int Id PK
        int PromotionId FK
        string ActionType 
        int TargetProductUnitId FK "nullable"
        decimal Value
        decimal MaxAmount "nullable"
    }

    PromoUsageLog {
        int Id PK
        int PromotionId FK
        int OrderId
        int CustomerId
        datetime UsedAt
        decimal DiscountAmount
    }

    PromotionCustomer {
        int PromotionId FK
        int CustomerId FK
    }

    PromotionProduct {
        int PromotionId FK
        int ProductUnitId FK
    }
