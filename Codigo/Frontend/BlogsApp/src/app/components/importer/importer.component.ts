import { Component, OnInit } from '@angular/core';
import { ImporterService } from 'src/app/services/importer.service';
import { IImporter } from 'src/app/models/importer.model';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-importer',
  templateUrl: './importer.component.html',
  styleUrls: ['./importer.component.scss']
})
export class ImporterComponent implements OnInit {

  importers: any[] = [];
  selectedImporter: string = '';
  path: string = '';

  constructor(private importerService: ImporterService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getImporters();
  }

  getImporters(): void {
    this.importerService.getImporter().subscribe((importers: any) => {
      this.importers = importers;
    });
  }

  postImporter(): void {
    const importer: IImporter = {
      importerName: this.selectedImporter,
      path: this.path
    };
    
    this.importerService.postImporter(this.selectedImporter, this.path).subscribe((res: any) => {
      this.toastr.success('se han importado los articulos con éxito');
      this.router.navigate(['/home']);
    }), (err: any) => {
      this.toastr.error(err.response, 'Error');
    };
  }

}
