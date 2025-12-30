# ✅ PriceRule Engine – Developer Checklist

> **Goal**  
Implement a deterministic, finance-controlled base pricing engine that resolves **exactly one BasePrice per ProductUnit**, before promotions and tax.

---

## 1. Input Contract (MANDATORY)

- [ ] Accept the following inputs:
  - [ ] `ProductUnitId` **(required)**
  - [ ] `OrderDate` **(required)**
  - [ ] `CurrencyId` **(required)**
  - [ ] `CustomerId` (optional)
  - [ ] `PriceGroupIds` (0..n)
  - [ ] `Quantity` (optional, future-proof)

- [ ] Reject execution if required inputs are missing

---

## 2. Output Contract (MANDATORY)

- [ ] Return **all** of the following:
  - [ ] `FinalBasePrice`
  - [ ] `AppliedRuleId`
  - [ ] `RuleType`
  - [ ] `ScopeType`
  - [ ] `ScopeId`
  - [ ] `CostPriceUsed`
  - [ ] `ResolutionMode` (`HIGHEST` | `LOWEST`)
  - [ ] `EvaluationTimestamp`

- [ ] Never return partial results

---

## 3. Cost Price Resolution (NON-NEGOTIABLE)

Resolve cost price in this exact order:

1. [ ] Latest valid `PurchasePrice` for `ProductUnit`
2. [ ] Fallback `StandardCost`
3. [ ] If no cost found → **HARD FAIL**

Must NOT:
- [ ] Average costs
- [ ] Estimate costs
- [ ] Use promotional prices

---

## 4. Rule Collection (MANDATORY)

- [ ] Collect **active** PriceRules (by `OrderDate`) from:
  - [ ] PRODUCTUNIT
  - [ ] PRODUCTVARIANT
  - [ ] PRODUCT
  - [ ] PRICE_GROUP
  - [ ] CUSTOMER
  - [ ] GLOBAL

- [ ] Ignore expired or future rules

---

## 5. Scope Validation (HARD RULE)

- [ ] Validate `RuleType × ScopeType` against validation matrix
- [ ] Reject rule if:
  - [ ] Scope is forbidden
  - [ ] RuleType is not allowed for that scope
- [ ] Invalid rules must NOT participate in evaluation

---

## 6. Rule Evaluation (MANDATORY)

For **each valid rule**:

- [ ] Calculate `CandidateBasePrice`
- [ ] Use resolved `CostPrice` where required
- [ ] Apply exact formula defined by RuleType
- [ ] Do NOT round unless `ROUNDING_OVERRIDE` exists

---

## 7. Floor & Ceiling Filtering

- [ ] Apply `PRICE_FLOOR`
  - [ ] Discard candidate prices below floor
- [ ] Apply `PRICE_CEILING`
  - [ ] Discard candidate prices above ceiling

> Floor and ceiling **filter candidates only** — they do not generate prices.

---

## 8. Cost Protection (CRITICAL)

Reject any candidate where:
CandidateBasePrice < CostPrice


- [ ] Exception allowed **only if explicitly flagged and audited**

---

## 9. Resolution Mode Selection

- [ ] Default resolution mode is **LOWEST_PRICE_WINS**

- [ ] `HIGHEST_PRICE_WINS` is allowed **only if at least one of the following is true**:
  - [ ] SalesChannel explicitly allows `HIGHEST_PRICE_WINS`
  - [ ] Customer contract explicitly flags `HIGHEST_PRICE_WINS`

- [ ] Finance approval exists for using `HIGHEST_PRICE_WINS`
- [ ] Finance approval is persisted (not runtime, not config-only)

- [ ] Resolution mode is determined internally
- [ ] Caller cannot pass resolution mode as a parameter
- [ ] Caller cannot override resolution mode via API, UI, or service call
- [ ] Any attempt to override resolution mode is rejected


---

## 10. Final Price Selection

- [ ] If no valid candidates exist:
  - [ ] Apply `GLOBAL_DEFAULT`
  - [ ] If missing → **HARD FAIL**

- [ ] Sort candidates:
  - [ ] DESC for `HIGHEST`
  - [ ] ASC for `LOWEST`

- [ ] Select **exactly one** price
- [ ] Stop evaluation immediately after selection

---

## 11. Rule Stacking (FORBIDDEN)

Must NOT:
- [ ] Stack rules
- [ ] Average prices
- [ ] Apply multiple adjustments

- [ ] Exactly **one rule result** must win

---

## 12. Time Awareness

- [ ] Evaluate pricing strictly **as-of OrderDate**
- [ ] Support historical price reconstruction

Must NOT:
- [ ] Use implicit current date
- [ ] Cache prices without OrderDate key

---

## 13. Error Handling Rules

| Condition | Action |
|--------|--------|
| Missing cost | HARD FAIL |
| Invalid rule configuration | HARD FAIL |
| Multiple GLOBAL_DEFAULT rules | HARD FAIL |
| No active rule | Use GLOBAL_DEFAULT |
| Below-cost price | Reject candidate |

---

## 14. Audit & Logging (MANDATORY)

Log per evaluation:

- [ ] RuleId
- [ ] RuleType
- [ ] ScopeType
- [ ] ScopeId
- [ ] CostPrice
- [ ] FinalBasePrice
- [ ] ResolutionMode
- [ ] Timestamp

Logs must support **financial audit**.

---

## 15. Database Constraints (REQUIRED)

Enforce at DB level:

- [ ] One active `GLOBAL_DEFAULT`
- [ ] No overlapping rules per:
  - ScopeType
  - ScopeId
  - RuleType
  - Date range
- [ ] CHECK constraints for allowed value ranges

---

## 16. Mandatory Test Scenarios

Implement automated tests for:

- [ ] Product vs Variant vs Unit conflict
- [ ] PriceGroup vs Customer conflict
- [ ] Floor blocking lowest price
- [ ] Ceiling blocking highest price
- [ ] Missing cost failure
- [ ] Historical pricing replay
- [ ] Resolution mode enforcement

---

## 17. Explicit Non-Goals (DO NOT IMPLEMENT)

- [ ] Quantity-based pricing
- [ ] Promotions
- [ ] Loyalty logic
- [ ] Dynamic pricing
- [ ] Competitor pricing
- [ ] AI optimization

---

## 18. Final Acceptance Criteria

- [ ] Exactly one BasePrice returned
- [ ] FinalBasePrice ≥ CostPrice (unless explicitly allowed)
- [ ] Promotion engine executes **after** PriceRule
- [ ] Tax engine executes **after** promotions
- [ ] Finance can fully audit pricing outcome

---

## 🧠 Final Developer Rule

> If you are unsure whether logic belongs here:  
> **It does not.**

`PriceRule` is **base pricing only** — nothing else.


