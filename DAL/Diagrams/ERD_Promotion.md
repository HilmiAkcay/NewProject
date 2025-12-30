```mermaid
erDiagram

    Promotion ||--o{ PromoCondition : has
    Promotion ||--o{ PromoAction : executes
    Promotion ||--o{ PromoUsageLog : tracked_by
    Promotion ||--o{ PromotionCustomer : limited_to
    Promotion ||--o{ PromotionProduct : limited_to

    Promotion {
        uuid Id PK
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
        uuid Id PK
        uuid PromotionId FK
        string ConditionType "MIN_QTY | MIN_AMOUNT | PRODUCT | CATEGORY | BRAND | CUSTOMER | PRICE_GROUP"
        string Operator "= | >= | IN"
        string Value "string or JSON"
    }

    PromoAction {
        uuid Id PK
        uuid PromotionId FK
        string ActionType "PERCENT_DISCOUNT | FIXED_DISCOUNT | FREE_ITEM | FIXED_PRICE"
        uuid TargetProductId FK "nullable"
        decimal Value
        decimal MaxAmount "nullable"
    }

    PromoUsageLog {
        uuid Id PK
        uuid PromotionId FK
        uuid OrderId
        uuid CustomerId
        datetime UsedAt
        decimal DiscountAmount
    }

    PromotionCustomer {
        uuid PromotionId FK
        uuid CustomerId FK
    }

    PromotionProduct {
        uuid PromotionId FK
        uuid ProductId FK
    }
