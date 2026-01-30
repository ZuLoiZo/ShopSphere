import { useEffect, useState } from 'react'
import { getProducts, Product, ProductListResponse } from '../services/productService'
import ProductCard from '../components/product/ProductCard'
import { Search } from 'lucide-react'
import toast from 'react-hot-toast'

export default function ProductsPage() {
  const [loading, setLoading] = useState(true)
  const [products, setProducts] = useState<Product[]>([])
  const [totalPages, setTotalPages] = useState(0)
  const [currentPage, setCurrentPage] = useState(1)
  const [search, setSearch] = useState('')

  useEffect(() => {
    loadProducts()
  }, [currentPage, search])

  const loadProducts = async () => {
    try {
      setLoading(true)
      const data: ProductListResponse = await getProducts({
        page: currentPage,
        pageSize: 12,
        search: search || undefined
      })
      setProducts(data.products)
      setTotalPages(data.totalPages)
    } catch (error) {
      console.error('Erreur lors du chargement des produits:', error)
      toast.error('Erreur lors du chargement des produits')
    } finally {
      setLoading(false)
    }
  }

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault()
    setCurrentPage(1)
    loadProducts()
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-8">
        <h1 className="text-4xl font-bold mb-4">Nos Produits</h1>
        
        <form onSubmit={handleSearch} className="flex gap-4 mb-6">
          <div className="flex-grow relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={20} />
            <input
              type="text"
              placeholder="Rechercher un produit..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500 outline-none"
            />
          </div>
          <button type="submit" className="btn-primary">
            Rechercher
          </button>
        </form>
      </div>

      {loading ? (
        <div className="text-center py-12">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
          <p className="mt-4 text-gray-600">Chargement des produits...</p>
        </div>
      ) : (
        <>
          {products.length === 0 ? (
            <div className="text-center py-12">
              <p className="text-gray-600 text-lg">Aucun produit trouvé</p>
            </div>
          ) : (
            <>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 mb-8">
                {products.map((product) => (
                  <ProductCard key={product.id} product={product} />
                ))}
              </div>

              {totalPages > 1 && (
                <div className="flex justify-center gap-2">
                  <button
                    onClick={() => setCurrentPage(prev => Math.max(1, prev - 1))}
                    disabled={currentPage === 1}
                    className="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    Précédent
                  </button>
                  
                  <span className="px-4 py-2">
                    Page {currentPage} sur {totalPages}
                  </span>
                  
                  <button
                    onClick={() => setCurrentPage(prev => Math.min(totalPages, prev + 1))}
                    disabled={currentPage === totalPages}
                    className="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    Suivant
                  </button>
                </div>
              )}
            </>
          )}
        </>
      )}
    </div>
  )
}
