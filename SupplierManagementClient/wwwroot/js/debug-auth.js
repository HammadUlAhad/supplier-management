/**
 * Exercise 3: Professional API Client Implementation
 * Demonstrates synchronous vs asynchronous API consumption patterns
 * Following industry best practices for JavaScript client applications
 * 
 * @author Exercise 3 Implementation
 * @version 3.0 - Production Ready
 */

class SupplierManagementClient {
    constructor() {
        console.log('🚀 Initializing SupplierManagementClient v3.0 - Production Ready');
        
        this.config = null;
        this.authToken = null;
        this.isAuthenticated = false;
        
        // Performance tracking
        this.metrics = {
            authTime: 0,
            syncCallTime: 0,
            asyncCallTime: 0
        };
        
        this.init();
    }
    
    /**
     * Initialize the client application
     */
    async init() {
        try {
            this.loadConfiguration();
            this.bindEventHandlers();
            this.checkExistingAuth();
            this.setupPerformanceMonitoring();
            
            console.log('✅ SupplierManagementClient initialized successfully');
            console.log('📊 Ready for Exercise 3 demonstration');
        } catch (error) {
            console.error('❌ Failed to initialize client:', error);
            this.displayError('Initialization Error', error.message);
        }
    }
    
    /**
     * Load and validate configuration from server-rendered data
     */
    loadConfiguration() {
        const configElement = document.getElementById('app-config');
        if (!configElement) {
            throw new Error('Configuration element not found');
        }
        
        try {
            this.config = JSON.parse(configElement.textContent);
            console.log('🔧 Configuration loaded:', {
                baseUrl: this.config.apiSettings.baseUrl,
                endpoints: Object.keys(this.config.apiSettings).filter(k => k.includes('Endpoint')),
                timestamp: this.config.timestamp
            });
        } catch (error) {
            throw new Error('Failed to parse configuration: ' + error.message);
        }
    }
    
    /**
     * Bind event handlers to UI elements
     */
    bindEventHandlers() {
        // Authentication button
        const authBtn = document.getElementById('btnAuthenticate');
        if (authBtn) {
            // Remove any existing handlers and replace the element to ensure clean binding
            const newAuthBtn = authBtn.cloneNode(true);
            authBtn.parentNode.replaceChild(newAuthBtn, authBtn);
            
            newAuthBtn.addEventListener('click', (e) => this.handleAuthentication(e));
            console.log('🔗 Authentication handler bound');
        }
        
        // Synchronous API call button
        const syncBtn = document.getElementById('btnGetSuppliers');
        if (syncBtn) {
            syncBtn.addEventListener('click', (e) => this.handleSynchronousCall(e));
            console.log('🔗 Synchronous API handler bound');
        }
        
        // Asynchronous API call button
        const asyncBtn = document.getElementById('btnGetOverlaps');
        if (asyncBtn) {
            asyncBtn.addEventListener('click', (e) => this.handleAsynchronousCall(e));
            console.log('🔗 Asynchronous API handler bound');
        }
        
        // Global error handler
        window.addEventListener('error', (event) => {
            console.error('💥 Unhandled error:', event.error);
            this.displayError('Application Error', event.error.message);
        });
    }
    
    /**
     * Check for existing authentication token
     */
    checkExistingAuth() {
        const token = sessionStorage.getItem('jwt_token');
        const expiry = sessionStorage.getItem('jwt_expiry');
        
        if (token && expiry && new Date(expiry) > new Date()) {
            this.authToken = token;
            this.isAuthenticated = true;
            this.updateAuthenticationStatus('success', '✅ Already authenticated');
            this.enableAuthenticatedFeatures();
            console.log('🔐 Found valid existing authentication');
        }
    }
    
    /**
     * Setup performance monitoring for demonstration purposes
     */
    setupPerformanceMonitoring() {
        // Monitor page load performance
        window.addEventListener('load', () => {
            const loadTime = performance.now();
            console.log(`📊 Page loaded in ${loadTime.toFixed(2)}ms`);
        });
    }
    
    /**
     * Handle authentication process
     * @param {Event} event - Click event
     */
    async handleAuthentication(event) {
        event.preventDefault();
        
        const startTime = performance.now();
        console.log('🔑 Starting authentication process...');
        
        const authBtn = document.getElementById('btnAuthenticate');
        const originalContent = authBtn.innerHTML;
        
        try {
            // Update UI to show loading state
            this.setButtonLoading(authBtn, 'Authenticating...');
            this.updateAuthenticationStatus('info', '🔄 Authenticating with server...');
            
            // Make authentication request
            const response = await fetch(`${this.config.apiSettings.baseUrl}${this.config.apiSettings.authEndpoint}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest' // Industry practice for AJAX identification
                },
                body: JSON.stringify({
                    username: this.config.credentials.username,
                    password: this.config.credentials.password
                })
            });
            
            // Handle response
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({ message: 'Authentication failed' }));
                throw new Error(errorData.message || `HTTP ${response.status}: ${response.statusText}`);
            }
            
            const data = await response.json();
            
            // Validate response structure (industry practice)
            if (!data.success || !data.token?.token) {
                throw new Error('Invalid authentication response format');
            }
            
            // Store authentication data
            this.authToken = data.token.token;
            this.isAuthenticated = true;
            
            sessionStorage.setItem('jwt_token', this.authToken);
            sessionStorage.setItem('jwt_expiry', data.token.expiration || new Date(Date.now() + 24*60*60*1000).toISOString());
            
            // Calculate performance metrics
            this.metrics.authTime = performance.now() - startTime;
            
            console.log(`✅ Authentication successful in ${this.metrics.authTime.toFixed(2)}ms`);
            console.log(`🔐 JWT Token: ${this.authToken.substring(0, 50)}...`);
            
            // Update UI
            this.updateAuthenticationStatus('success', `✅ Authenticated successfully (${this.metrics.authTime.toFixed(0)}ms)`);
            this.enableAuthenticatedFeatures();
            
        } catch (error) {
            console.error('❌ Authentication failed:', error);
            this.updateAuthenticationStatus('danger', `❌ Authentication failed: ${error.message}`);
            this.displayError('Authentication Error', error.message);
        } finally {
            // Restore button state
            authBtn.disabled = false;
            authBtn.innerHTML = originalContent;
        }
    }
    
    /**
     * Handle SYNCHRONOUS API call (Exercise 3 requirement)
     * Demonstrates blocking UI behavior
     * @param {Event} event - Click event
     */
    handleSynchronousCall(event) {
        event.preventDefault();
        
        if (!this.validateAuthentication()) return;
        
        const startTime = performance.now();
        console.log('🔄 Starting SYNCHRONOUS API call (XMLHttpRequest)...');
        console.warn('⚠️ UI WILL FREEZE during this synchronous operation');
        
        // Update UI before blocking operation
        this.updateSyncStatus('warning', '⚠️ SYNCHRONOUS: UI is blocked during this call');
        this.disableAllButtons();
        this.showLoadingIndicator('Making synchronous call - UI will freeze...');
        
        try {
            // Use XMLHttpRequest for true synchronous behavior (Exercise 3 requirement)
            const xhr = new XMLHttpRequest();
            const url = `${this.config.apiSettings.baseUrl}${this.config.apiSettings.suppliersEndpoint}`;
            
            // Configure synchronous request (false = synchronous)
            xhr.open('GET', url, false);
            xhr.setRequestHeader('Authorization', `Bearer ${this.authToken}`);
            xhr.setRequestHeader('Accept', 'application/json');
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            
            console.log(`📡 Making synchronous request to: ${url}`);
            
            // This line will BLOCK the UI thread (demonstrating synchronous behavior)
            xhr.send();
            
            // Handle response
            if (xhr.status === 200) {
                const data = JSON.parse(xhr.responseText);
                this.metrics.syncCallTime = performance.now() - startTime;
                
                console.log(`✅ Synchronous call completed in ${this.metrics.syncCallTime.toFixed(2)}ms`);
                console.log(`📦 Received ${data.suppliers?.length || 0} suppliers`);
                
                // Display results
                this.displayApiResults('Suppliers & Rates (Synchronous Call)', data, 'sync');
                this.updateSyncStatus('success', `✅ Synchronous call completed (${this.metrics.syncCallTime.toFixed(0)}ms)`);
                
            } else {
                throw new Error(`HTTP ${xhr.status}: ${xhr.statusText}`);
            }
            
        } catch (error) {
            console.error('❌ Synchronous call failed:', error);
            this.updateSyncStatus('danger', `❌ Synchronous call failed: ${error.message}`);
            this.displayError('Synchronous API Error', error.message);
        } finally {
            this.hideLoadingIndicator();
            this.enableAllButtons();
        }
    }
    
    /**
     * Handle ASYNCHRONOUS API call (Exercise 3 requirement)
     * Demonstrates non-blocking UI behavior
     * @param {Event} event - Click event
     */
    async handleAsynchronousCall(event) {
        event.preventDefault();
        
        if (!this.validateAuthentication()) return;
        
        const startTime = performance.now();
        console.log('⚡ Starting ASYNCHRONOUS API call (Fetch API)...');
        console.log('✅ UI remains responsive during this asynchronous operation');
        
        // Update UI (non-blocking)
        this.updateAsyncStatus('info', '🔄 ASYNCHRONOUS: UI remains responsive');
        this.showLoadingIndicator('Making asynchronous call - UI remains responsive...');
        
        try {
            const url = `${this.config.apiSettings.baseUrl}${this.config.apiSettings.overlapsEndpoint}`;
            console.log(`📡 Making asynchronous request to: ${url}`);
            
            // Use Fetch API for modern asynchronous behavior
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${this.authToken}`,
                    'Accept': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
            
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }
            
            const data = await response.json();
            this.metrics.asyncCallTime = performance.now() - startTime;
            
            console.log(`✅ Asynchronous call completed in ${this.metrics.asyncCallTime.toFixed(2)}ms`);
            console.log(`📦 Found ${data.overlappingRates?.length || 0} overlapping rate periods`);
            
            // Display results
            this.displayApiResults('Overlapping Rates (Asynchronous Call)', data, 'async');
            this.updateAsyncStatus('success', `✅ Asynchronous call completed (${this.metrics.asyncCallTime.toFixed(0)}ms)`);
            
        } catch (error) {
            console.error('❌ Asynchronous call failed:', error);
            this.updateAsyncStatus('danger', `❌ Asynchronous call failed: ${error.message}`);
            this.displayError('Asynchronous API Error', error.message);
        } finally {
            this.hideLoadingIndicator();
        }
    }
    
    /**
     * Validate authentication before API calls
     * @returns {boolean} - Authentication status
     */
    validateAuthentication() {
        if (!this.isAuthenticated || !this.authToken) {
            this.displayError('Authentication Required', 'Please authenticate before making API calls');
            return false;
        }
        
        // Check token expiry
        const expiry = sessionStorage.getItem('jwt_expiry');
        if (expiry && new Date(expiry) <= new Date()) {
            this.isAuthenticated = false;
            this.authToken = null;
            sessionStorage.removeItem('jwt_token');
            sessionStorage.removeItem('jwt_expiry');
            this.updateAuthenticationStatus('warning', '⚠️ Session expired - please re-authenticate');
            return false;
        }
        
        return true;
    }
    
    /**
     * Display API results in the UI
     * @param {string} title - Result title
     * @param {Object} data - API response data
     * @param {string} type - Call type (sync/async)
     */
    displayApiResults(title, data, type) {
        const resultsContainer = document.getElementById('apiResults');
        if (!resultsContainer) return;
        
        const timestamp = new Date().toLocaleTimeString();
        const callTypeIcon = type === 'sync' ? '🔄' : '⚡';
        const callTypeBadge = type === 'sync' ? 'bg-warning' : 'bg-primary';
        const callTypeText = type === 'sync' ? 'Synchronous' : 'Asynchronous';
        
        // Create professional results display
        resultsContainer.innerHTML = `
            <div class="results-header mb-3">
                <div class="d-flex justify-content-between align-items-center">
                    <h6 class="text-primary mb-0">
                        ${callTypeIcon} ${title}
                        <span class="badge ${callTypeBadge} ms-2">${callTypeText}</span>
                    </h6>
                    <small class="text-muted">
                        <i class="fas fa-clock me-1"></i>
                        ${timestamp}
                    </small>
                </div>
                <div class="mt-2">
                    <small class="text-muted">
                        Performance: ${type === 'sync' ? this.metrics.syncCallTime.toFixed(2) : this.metrics.asyncCallTime.toFixed(2)}ms
                        | Records: ${this.getRecordCount(data)}
                        | Type: ${type === 'sync' ? 'XMLHttpRequest (Blocking)' : 'Fetch API (Non-blocking)'}
                    </small>
                </div>
            </div>
            
            <div class="results-summary mb-3">
                <div class="row">
                    <div class="col-md-6">
                        <div class="card bg-light">
                            <div class="card-body p-2">
                                <h6 class="card-title mb-1">
                                    <i class="fas fa-info-circle text-info me-1"></i>
                                    Exercise 3 Demonstration
                                </h6>
                                <p class="card-text small mb-0">
                                    ${type === 'sync' 
                                        ? 'This call was made <strong>synchronously</strong> using XMLHttpRequest. The UI was blocked during execution.'
                                        : 'This call was made <strong>asynchronously</strong> using Fetch API. The UI remained responsive during execution.'
                                    }
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="card bg-light">
                            <div class="card-body p-2">
                                <h6 class="card-title mb-1">
                                    <i class="fas fa-chart-line text-success me-1"></i>
                                    Performance Metrics
                                </h6>
                                <p class="card-text small mb-0">
                                    <strong>Auth:</strong> ${this.metrics.authTime.toFixed(0)}ms |
                                    <strong>Sync:</strong> ${this.metrics.syncCallTime.toFixed(0)}ms |
                                    <strong>Async:</strong> ${this.metrics.asyncCallTime.toFixed(0)}ms
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="results-data">
                <div class="accordion" id="resultsAccordion">
                    <div class="accordion-item">
                        <h2 class="accordion-header" id="headingData">
                            <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseData" aria-expanded="true" aria-controls="collapseData">
                                <i class="fas fa-database me-2"></i>
                                Raw API Response Data
                            </button>
                        </h2>
                        <div id="collapseData" class="accordion-collapse collapse show" aria-labelledby="headingData" data-bs-parent="#resultsAccordion">
                            <div class="accordion-body">
                                <pre class="bg-dark text-light p-3 rounded" style="max-height: 400px; overflow-y: auto;"><code>${JSON.stringify(data, null, 2)}</code></pre>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        // Show metrics badge
        const metricsBadge = document.getElementById('responseMetrics');
        if (metricsBadge) {
            metricsBadge.textContent = `${this.getRecordCount(data)} records`;
            metricsBadge.style.display = 'inline';
        }
    }
    
    /**
     * Get record count from API response
     * @param {Object} data - API response
     * @returns {number} - Record count
     */
    getRecordCount(data) {
        if (data.suppliers) return data.suppliers.length;
        if (data.overlappingRates) return data.overlappingRates.length;
        if (Array.isArray(data)) return data.length;
        return 1;
    }
    
    // UI Helper Methods
    
    updateAuthenticationStatus(type, message) {
        const authStatus = document.getElementById('authStatus');
        if (authStatus) {
            authStatus.className = `alert alert-${type} mb-0`;
            authStatus.innerHTML = `<i class="fas fa-info-circle me-2"></i>${message}`;
        }
    }
    
    updateSyncStatus(type, message) {
        const syncStatus = document.getElementById('syncStatus');
        if (syncStatus) {
            syncStatus.innerHTML = `<small class="text-${type}">${message}</small>`;
        }
    }
    
    updateAsyncStatus(type, message) {
        const asyncStatus = document.getElementById('asyncStatus');
        if (asyncStatus) {
            asyncStatus.innerHTML = `<small class="text-${type}">${message}</small>`;
        }
    }
    
    enableAuthenticatedFeatures() {
        const syncBtn = document.getElementById('btnGetSuppliers');
        const asyncBtn = document.getElementById('btnGetOverlaps');
        
        if (syncBtn) {
            syncBtn.disabled = false;
            syncBtn.querySelector('small')?.remove();
        }
        if (asyncBtn) {
            asyncBtn.disabled = false;
            asyncBtn.querySelector('small')?.remove();
        }
    }
    
    disableAllButtons() {
        ['btnAuthenticate', 'btnGetSuppliers', 'btnGetOverlaps'].forEach(id => {
            const btn = document.getElementById(id);
            if (btn) btn.disabled = true;
        });
    }
    
    enableAllButtons() {
        const authBtn = document.getElementById('btnAuthenticate');
        if (authBtn) authBtn.disabled = false;
        
        if (this.isAuthenticated) {
            this.enableAuthenticatedFeatures();
        }
    }
    
    setButtonLoading(button, text) {
        button.disabled = true;
        button.innerHTML = `<i class="fas fa-spinner fa-spin me-2"></i>${text}`;
    }
    
    showLoadingIndicator(message) {
        const loadingSection = document.getElementById('loadingSection');
        const loadingMessage = document.getElementById('loadingMessage');
        
        if (loadingSection) loadingSection.style.display = 'block';
        if (loadingMessage) loadingMessage.textContent = message;
    }
    
    hideLoadingIndicator() {
        const loadingSection = document.getElementById('loadingSection');
        if (loadingSection) loadingSection.style.display = 'none';
    }
    
    displayError(title, message) {
        console.error(`❌ ${title}: ${message}`);
        
        // Could implement toast notifications or modal dialogs here
        // For now, using console and alert for simplicity
        if (message.includes('Authentication') || message.includes('authenticate')) {
            // Don't show alert for auth errors, just log them
            return;
        }
        
        alert(`${title}: ${message}`);
    }
}

// Initialize the client when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    console.log('🚀 Exercise 3: Professional API Client Implementation');
    console.log('📋 Requirements:');
    console.log('   ✅ Separate project for web page');
    console.log('   ✅ Client scripting for API calls');
    console.log('   ✅ Synchronous call for suppliers & rates');
    console.log('   ✅ Asynchronous call for overlapping rates');
    console.log('   ✅ Event-triggered operations');
    console.log('   ✅ Visible data display');
    
    try {
        // Create global instance following industry practices
        window.supplierClient = new SupplierManagementClient();
        
        // Add manual testing functions for debugging
        window.testAuth = () => window.supplierClient.handleAuthentication({ preventDefault: () => {} });
        window.testSync = () => window.supplierClient.handleSynchronousCall({ preventDefault: () => {} });
        window.testAsync = () => window.supplierClient.handleAsynchronousCall({ preventDefault: () => {} });
        
        console.log('🔧 Manual testing functions available:');
        console.log('   - window.testAuth() - Test authentication');
        console.log('   - window.testSync() - Test synchronous call');
        console.log('   - window.testAsync() - Test asynchronous call');
        
    } catch (error) {
        console.error('❌ Failed to initialize SupplierManagementClient:', error);
    }
});
