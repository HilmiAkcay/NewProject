erDiagram
    PROMOTION ||--o{ PROMO_CONDITION : triggers_on
    PROMOTION ||--o{ PROMO_ACTION : executes
    PROMOTION ||--o{ PROMO_STORE_LINK : active_in
    
    PROMO_CONDITION {
        uuid id PK
        uuid promo_id FK
        string match_type "MIN_QTY | EXACT_QTY | MULTIPLE_OF"
        integer required_value "e.g., 6"
        string scope "SKU | CATEGORY | BRAND | ALL"
        uuid reference_id "ID of Product or Category"
    }

    PROMO_ACTION {
        uuid id PK
        uuid promo_id FK
        string calc_type "PERCENT | FIXED_AMOUNT | MARGIN_TARGET | GIFT"
        decimal value "e.g., 10.0 or 50.0"
        string apply_to "ALL_ITEMS | CHEAPEST | MOST_EXPENSIVE"
        uuid gift_sku "Optional SKU for free items"
    }

    PROMOTION {
        uuid id PK
        string name
        string type "B2C_PROMO | B2B_AGREEMENT"
        integer priority "Higher = overrides others"
        boolean is_stackable
        string customer_category "e.g., Cat 1, VIP, All"
        datetime start_date
        datetime end_date
    }
