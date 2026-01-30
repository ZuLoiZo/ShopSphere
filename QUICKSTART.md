# 🚀 Guide de Démarrage Rapide - ShopSphere

## ⚡ Lancement rapide (avec Docker)

### 1. Configuration initiale

```bash
# Copier le fichier d'environnement
cp .env.example .env

# Éditer .env et mettre vos valeurs (notamment JWT_SECRET et clés Stripe)
```

### 2. Lancer tout avec Docker

```bash
# Démarrer tous les services
docker-compose up -d

# Voir les logs
docker-compose logs -f
```

✅ **C'est tout !** L'application est maintenant accessible :
- Frontend : http://localhost:3000
- Backend API : http://localhost:5000
- Swagger : http://localhost:5000/swagger
- pgAdmin : http://localhost:5050 (admin@shopsphere.com / admin)

---

## 🛠️ Installation manuelle (sans Docker)

### Prérequis

- Node.js 20+
- .NET 8 SDK
- PostgreSQL 15+
- Git

### 1. Configuration de la base de données

```bash
# Créer la base de données
createdb shopsphere

# Ou via psql
psql -U postgres
CREATE DATABASE shopsphere;
\q
```

### 2. Backend (.NET)

```bash
# Aller dans le dossier backend
cd backend

# Restaurer les dépendances
dotnet restore

# Créer les migrations et la base
cd ShopSphere.API
dotnet ef database update --project ../ShopSphere.Infrastructure

# Lancer l'API
dotnet run

# L'API est maintenant sur http://localhost:5000
# Swagger sur http://localhost:5000/swagger
```

### 3. Frontend (React)

Ouvrir un nouveau terminal :

```bash
# Aller dans le dossier frontend
cd frontend

# Copier l'environnement
cp .env.example .env

# Installer les dépendances
npm install

# Lancer en mode développement
npm run dev

# L'application est sur http://localhost:3000
```

---

## 📝 Configuration essentielle

### Fichier `.env` à la racine

```env
DATABASE_URL="Host=localhost;Database=shopsphere;Username=postgres;Password=postgres"
JWT_SECRET=votre-secret-jwt-minimum-32-caracteres-tres-securise
JWT_EXPIRATION_MINUTES=15
JWT_REFRESH_EXPIRATION_DAYS=7
STRIPE_SECRET_KEY=sk_test_votre_cle_stripe
STRIPE_PUBLISHABLE_KEY=pk_test_votre_cle_stripe
```

### Fichier `frontend/.env`

```env
VITE_API_URL=http://localhost:5000
VITE_STRIPE_PUBLISHABLE_KEY=pk_test_votre_cle_stripe
```

---

## 🗄️ Créer les migrations (si nécessaire)

```bash
cd backend/ShopSphere.API

# Créer une nouvelle migration
dotnet ef migrations add NomDeLaMigration --project ../ShopSphere.Infrastructure

# Appliquer les migrations
dotnet ef database update --project ../ShopSphere.Infrastructure
```

---

## 🎯 Prochaines étapes de développement

### Phase 1 : Backend Core (Jour 1-3)

1. **Créer les services**
   - `AuthService.cs` - Authentification JWT
   - `ProductService.cs` - CRUD produits
   - `CartService.cs` - Gestion du panier
   
2. **Créer les controllers**
   - `AuthController.cs` - Register, Login, Refresh
   - `ProductsController.cs` - CRUD produits
   - `CartController.cs` - Gestion panier

3. **Créer les DTOs**
   - `LoginDto`, `RegisterDto`, `TokenDto`
   - `ProductDto`, `CreateProductDto`
   - `CartDto`, `AddToCartDto`

### Phase 2 : Backend Avancé (Jour 4-5)

1. **Intégration Stripe**
   - Service de paiement
   - OrderController
   
2. **Seeding de données**
   - Créer des catégories
   - Créer des produits de test

### Phase 3 : Frontend Core (Jour 6-8)

1. **Services API**
   - `authService.ts`
   - `productService.ts`
   - `cartService.ts`

2. **Pages complètes**
   - Login/Register avec formulaires
   - Liste des produits avec filtres
   - Détail produit
   - Panier fonctionnel

### Phase 4 : Intégration (Jour 9-10)

1. **Connecter Frontend ↔ Backend**
2. **Tests d'intégration**
3. **Corrections de bugs**

### Phase 5 : Déploiement (Jour 11-12)

1. **GitHub Actions CI/CD**
2. **Déploiement Heroku**
3. **Documentation finale**

---

## 🐛 Résolution de problèmes

### Le backend ne démarre pas

```bash
# Vérifier que PostgreSQL est démarré
sudo service postgresql status

# Vérifier les migrations
cd backend/ShopSphere.API
dotnet ef database update --project ../ShopSphere.Infrastructure
```

### Le frontend ne se connecte pas au backend

1. Vérifier que le backend tourne sur le port 5000
2. Vérifier le fichier `frontend/.env`
3. Vérifier les CORS dans `backend/ShopSphere.API/Program.cs`

### Erreur de migration

```bash
# Supprimer la base et recommencer
dropdb shopsphere
createdb shopsphere
cd backend/ShopSphere.API
dotnet ef database update --project ../ShopSphere.Infrastructure
```

---

## 📚 Ressources utiles

- **Documentation .NET** : https://learn.microsoft.com/en-us/aspnet/core/
- **React Documentation** : https://react.dev/
- **Redux Toolkit** : https://redux-toolkit.js.org/
- **Stripe Docs** : https://stripe.com/docs
- **TailwindCSS** : https://tailwindcss.com/docs

---

## ✅ Checklist avant de commencer à coder

- [ ] PostgreSQL installé et démarré
- [ ] .NET 8 SDK installé
- [ ] Node.js 20+ installé
- [ ] Compte Stripe créé (mode test)
- [ ] Fichiers `.env` configurés
- [ ] Base de données créée
- [ ] Migrations appliquées
- [ ] Backend démarre sans erreur
- [ ] Frontend démarre sans erreur
- [ ] Swagger accessible

---

**Prêt à coder ? Let's go ! 🚀**
