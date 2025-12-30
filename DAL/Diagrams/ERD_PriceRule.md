# PriceRule Diagram

### “What should the base sales price be before promotions?”
- [All Possible Rules](PriceRuleRules.md)
- [Developer CheckList](PriceRule_Developer.md)

  
```mermaid
erDiagram

    PriceRule ||--o{ PriceRuleScope : applies_to

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
