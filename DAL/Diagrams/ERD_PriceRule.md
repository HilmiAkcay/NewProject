### “What should the base sales price be before promotions?”

```mermaid
erDiagram

    PriceRule ||--o{ PriceRuleScope : applies_to
    ProductCost ||--|| Product : defines
    Product ||--o{ PriceRuleScope : referenced_by

    PriceRule {
        uuid Id PK
        string RuleType "MARGIN | FIXED_PRICE | DISCOUNT"
        decimal Value
        int Priority
        datetime ValidFrom
        datetime ValidTo
    }

    PriceRuleScope {
        uuid Id PK
        uuid PriceRuleId FK
        string ScopeType "PRODUCT | CATEGORY | BRAND | PRICE_GROUP | CUSTOMER"
        uuid ScopeId
    }

    ProductCost {
        uuid ProductId PK
        decimal CostPrice
        datetime ValidFrom
    }

    Product {
        uuid Id PK
        string Name
        string CategoryId
        string BrandId
    }
