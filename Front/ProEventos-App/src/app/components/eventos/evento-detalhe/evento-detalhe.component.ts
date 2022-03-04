import { TemplateParseResult } from '@angular/compiler';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  modalRef!: BsModalRef;
  eventoId!: number;
  evento = {} as Evento;
  estadoSalvar = 'post';
  form!: FormGroup;
  loteAtual = {id: 0, nome: '', indice: 0};

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
        isAnimated: true,
        adaptivePosition: true,
        dateInputFormat: 'DD/MM/YYYY hh:mm a',
        containerClass: 'theme-default',
        showWeekNumbers: false
    };
  }

  get bsConfigLote(): any {
    return {
        isAnimated: true,
        adaptivePosition: true,
        dateInputFormat: 'DD/MM/YYYY',
        containerClass: 'theme-default',
        showWeekNumbers: false
    };
  }

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private activetedRouter: ActivatedRoute,
              private eventoService: EventoService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private modalService: BsModalService,
              private router: Router,
              private loteService: LoteService)
  {
    this.localeService.use("pt-br");
  }

  public carregarEvento(): void {
    this.eventoId = +this.activetedRouter.snapshot.paramMap.get('id')!;

    if (this.eventoId !== null || this.eventoId !== 0) {
      this.spinner.show();
      this.estadoSalvar = 'put';
      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
          this.carregarLotes();
          // this.evento.lotes.forEach(lote => {
          //   this.lotes.push(this.criarLote(lote));
          // })
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar carregar evento.','Erro');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public carregarLotes(): void{
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes.push(this.criarLote(lote));
        })
      },
      (error: any) => {
        this.toastr.error('Erro ao recuperar lotes', 'Erro');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  ngOnInit() {
    this.carregarEvento();
    this.validation();
  }

  public validation():void{
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', [Validators.required]],
      telefone: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

  adicionarLote(): void {
    this.lotes.push(
      this.criarLote({id: 0} as Lote)
    );
  }

  private criarLote(lote: Lote) {
    return this.fb.group({
      id: [lote.id, Validators.required],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }

  public resetForm(): void{
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl | null) : any{
    return {'is-invalid': campoForm?.errors && campoForm?.touched }
  }

  public salvarEvento(): void{
    this.spinner.show();
    if(this.form.valid){

      //Aqui foi refatorado -> Não funcionou pra mim.
      //rever no final.
      if(this.estadoSalvar === 'post'){
        this.evento = {...this.form.value};
        this.eventoService.post(this.evento).subscribe(
          (eventoRetorno: Evento) => {
                  this.toastr.success('Evento cadastrado.','Sucesso');
                  this.router.navigate( [`eventos/detalhe/${eventoRetorno.id}`]);
                },
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Ocorreu um erro.','Erro');
          },
          () => this.spinner.hide(),
        );
      }else{
        this.evento = {id: this.evento.id, ...this.form.value};
        this.eventoService.put(this.evento).subscribe(
          () => this.toastr.success('Evento cadastrado.','Sucesso'),
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Ocorreu um erro.','Erro');
          },
          () => this.spinner.hide(),
        );
      }
    }
  }

  public salvarLotes(): void {
    if(this.form.controls['lotes'].valid){
      this.spinner.show();
      this.loteService.saveLotes(this.eventoId, this.form.value.lotes)
      .subscribe(
        () => {
          this.toastr.success('Lotes Salvos.','Sucesso');
          this.lotes.reset();
        },
        (error: any) => {
          this.toastr.error('Ocorreu um erro.','Erro');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number) : void{
    this.loteAtual.id = this.lotes.get(indice + '.id')?.value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome')?.value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
    this.lotes.removeAt(indice);
  }

  public confirmDeleteLote(): void{
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id)
        .subscribe(
          () => {
            this.toastr.success('Lote excluído com Sucesso', 'Exclusão de Lote');
            this.lotes.removeAt(this.loteAtual.indice);
          },
          (error: any) => {
            this.toastr.error('Erro na exclusão de lote', 'Erro exclusão de Lote');
            console.error(error);
          }
        ).add(() => this.spinner.hide());

  }

  public declineDeleteLote(): void{
    this.modalRef.hide();
  }

  retornaTituloLote(nome: string):string {
    return nome === null || nome === '' ? 'Nome do Lote' : nome;
  }

}
