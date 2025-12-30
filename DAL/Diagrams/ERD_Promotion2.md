# Promotion & Pricing Model (Final)

## Core Pricing Rules

1. **Lowest final price always wins**
2. Priority is used **only** for override scenarios (e.g. B2B contracts)
3. Exclusive promotions stop further evaluation
4. Stackable promotions are combined, then compared by final price

---

## Promotion Evaluation Flow (Summary)

1. Filter eligible promotions (date, status, customer)
2. Evaluate conditions
3. Apply actions
4. Calculate final price per option
5. Select the **lowest payable price**

---
## Official Condition Types
- MIN_QTY_FROM_TARGET        // Sum of quantities across all PromotionTargets
- MIN_AMOUNT_FROM_TARGET    // Sum of line totals across targets
- EACH_TARGET_MIN_QTY       // Each target must meet qty (e.g. Buy A+B+C)
- CUSTOMER_IN
- PRICE_GROUP_IN
- TIME_RANGE

## Official Action Types
- PERCENT_DISCOUNT
- FIXED_DISCOUNT
- FREE_ITEM
- FIXED_PRICE


## Entity Relationship Diagram

```mermaid
erDiagram

    Promotion ||--o{ PromotionTarget : defines
    Promotion ||--o{ PromoCondition : requires
    Promotion ||--o{ PromoAction : grants
    Promotion ||--o{ PromotionCustomer : limited_to
    Promotion ||--o{ PromoUsageLog : tracked_by

    Promotion {
        int Id PK
        string Code
        string Name
        string Scope "B2C | B2B"
        int Priority "Higher = stronger override"
        boolean IsStackable
        boolean IsExclusive
        string Status "Draft | Active | Paused | Expired"
        datetime StartDate
        datetime EndDate
        datetime CreatedAt
    }

    PromotionTarget {
        int Id PK
        int PromotionId FK
        string TargetType "PRODUCT_UNIT | CATEGORY | BRAND | BUNDLE"
        int TargetId
    }

    PromoCondition {
        int Id PK
        int PromotionId FK
        string ConditionType
        string Operator "= | >= | IN"
        string Value "JSON"
    }

    PromoAction {
        int Id PK
        int PromotionId FK
        string ActionType "PERCENT_DISCOUNT | FIXED_DISCOUNT | FREE_ITEM | FIXED_PRICE"
        decimal Value
        decimal MaxAmount "Nullable"
        int MaxQty "Nullable"
    }

    PromotionCustomer {
        int PromotionId FK
        int CustomerId FK
    }

    PromoUsageLog {
        int Id PK
        int PromotionId FK
        int OrderId
        int CustomerId
        datetime UsedAt
        decimal DiscountAmount
    }

    Bundle ||--o{ BundleItem : contains

    Bundle {
        int Id PK
        string Name
        boolean IsActive
    }

    BundleItem {
        int BundleId FK
        int ProductUnitId FK
        int Qty
    }
