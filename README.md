# .net-core-api
# ⏱️ 3–4 Hour Practical Round Timeline

| Time        | Task                                            | Priority          |
| ----------- | ----------------------------------------------- | ----------------- |
| 0–15 min    | Create project + folders + install EF packages  | 🔥 Critical       |
| 15–30 min   | Setup DbContext + connection string + migration | 🔥 Critical       |
| 30–50 min   | Create Models + Relationships                   | 🔥 Critical       |
| 50–80 min   | Repository + Service Layer + DI                 | 🔥 Critical       |
| 80–110 min  | CRUD APIs/Views working                         | 🔥 Critical       |
| 110–130 min | DTOs + Mapping                                  | 🔥 High           |
| 130–150 min | Pagination + Search + IQueryable                | 🔥 High           |
| 150–165 min | Validation + Error handling                     | 🔥 Medium         |
| 165–180 min | Testing + Cleanup + Refactor                    | 🔥 Very Important |

---

# 🔥 Folder Creation Order

```text id="chart001"
Models
↓
Data
↓
Repositories
↓
Services
↓
Controllers
↓
DTOs
↓
Validators
```

---

# 🔥 Coding Flow

```text id="chart002"
Model
↓
DbContext
↓
Migration
↓
Repository
↓
Service
↓
Controller
↓
Test
```

---

# 🔥 Feature Priority

| Feature        | Must Have? |
| -------------- | ---------- |
| CRUD           | ✅          |
| Relationships  | ✅          |
| DTOs           | ✅          |
| Pagination     | ✅          |
| Validation     | ✅          |
| Error handling | ✅          |
| Authentication | ⚠️ If time |
| Logging        | ⚠️ Bonus   |
| Dapper         | ⚠️ Bonus   |

---

# 🔥 Most Important Rule

```text id="chart003"
Make functionality work FIRST
then improve architecture gradually.
```

---

# 🔥 Golden Rule

```text id="chart004"
Working clean project
>
Half-finished advanced architecture
```
