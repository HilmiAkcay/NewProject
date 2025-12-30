### “What should the base sales price be before promotions?”

## PriceRule Resolution Order (Strict)

When resolving a price for a **ProductUnit**, rules are evaluated in the following order.  
The **first applicable rule wins**. No stacking is allowed.

 ScopeTypes below

1. **PRODUCTUNIT** *(most specific)*
2. **PRODUCTVARIANT**
3. **PRODUCT**
4. **PRICE_GROUP**
5. **CUSTOMER**
6. **GLOBAL** *(optional, fallback only)*

> Pricing is always **resolved at ProductUnit level**, even when a rule is defined higher in the hierarchy.


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
